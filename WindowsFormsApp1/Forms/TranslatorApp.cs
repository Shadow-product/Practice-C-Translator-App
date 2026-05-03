using System;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Repositories;
using WindowsFormsApp1.Services;

namespace WindowsFormsApp1.Forms
{
    public partial class TranslatorApp : Form
    {
        // ── Плейсхолдеры ─────────────────────────────────────────────
        private const string SourcePlaceholder = "Введите текст для перевода...";
        private const string TargetPlaceholder = "Здесь появится перевод...";

        private readonly System.Drawing.Color _placeholderColor = System.Drawing.Color.DimGray;
        private readonly System.Drawing.Color _textColor = System.Drawing.Color.Gainsboro;

        // ── Перетаскивание безрамочного окна ─────────────────────────
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")] // ← новый
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        [DllImport("user32.dll")]
        private static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        // ── Сервисы и репозитории ─────────────────────────────────────
        private TranslationService _translationService;
        private TranslationRepository _translationRepository;
        private UserRepository _userRepository;

        public TranslatorApp()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["TranslatorAppDb"].ConnectionString;
            _translationRepository = new TranslationRepository(connectionString);
            _userRepository = new UserRepository(connectionString);

            string apiKey = ConfigurationManager.AppSettings["ApiKey"];
            _translationService = new TranslationService(apiKey);
        }

        private void TranslatorApp_Load(object sender, EventArgs e)
        {
            SetRoundedCorners(20);
            cbTargetLang.SelectedIndex = 1;
            // Загрузка иконки
            try
            {
                string iconPath = Path.Combine(Application.StartupPath, "deepl.ico");
                if (File.Exists(iconPath))
                    this.Icon = new System.Drawing.Icon(iconPath);
            }
            catch { }

            // Проверка БД
            try
            {
                var cs = ConfigurationManager.ConnectionStrings["TranslatorAppDb"].ConnectionString;
                _translationRepository = new TranslationRepository(cs);
                lblStatus.Text = _translationRepository.CheckDbConnection()
                    ? "База данных: Подключено"
                    : "База данных: Ошибка";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "База данных: Ошибка";
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
            }
        }

        // ── Перетаскивание окна ───────────────────────────────────────

        private void titleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        // ── Кнопки управления окном ───────────────────────────────────

        private void btnClose_Click(object sender, EventArgs e) => this.Close();
        private void btnMinimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        // ── Swap: поменять тексты местами ────────────────────────────

        private void btnSwap_Click(object sender, EventArgs e)
        {
            // Не свапаем если в источнике только плейсхолдер
            if (txtSource.Text == SourcePlaceholder)
                return;

            string sourceText = txtSource.Text;
            string targetText = txtTarget.Text == TargetPlaceholder ? "" : txtTarget.Text;

            // Меняем текст источника
            if (string.IsNullOrWhiteSpace(targetText))
            {
                txtSource.Text = SourcePlaceholder;
                txtSource.ForeColor = _placeholderColor;
            }
            else
            {
                txtSource.Text = targetText;
                txtSource.ForeColor = _textColor;
            }

            // Меняем текст перевода
            if (string.IsNullOrWhiteSpace(sourceText))
            {
                txtTarget.Text = TargetPlaceholder;
                txtTarget.ForeColor = _placeholderColor;
            }
            else
            {
                txtTarget.Text = sourceText;
                txtTarget.ForeColor = _textColor;
            }

            UpdateCharCount();
        }

        // ── Копировать перевод ────────────────────────────────────────

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (txtTarget.Text == TargetPlaceholder || string.IsNullOrWhiteSpace(txtTarget.Text))
            {
                lblStatus.Text = "Нечего копировать";
                return;
            }

            Clipboard.SetText(txtTarget.Text);

            // Кратко показываем подтверждение в кнопке
            btnCopy.Text = "✓  Скопировано";
            var timer = new System.Windows.Forms.Timer { Interval = 1500 };
            timer.Tick += (s, ev) =>
            {
                btnCopy.Text = "📋  Копировать";
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        // ── Плейсхолдер txtSource ─────────────────────────────────────

        private void txtSource_Enter(object sender, EventArgs e)
        {
            if (txtSource.Text == SourcePlaceholder)
            {
                txtSource.Text = "";
                txtSource.ForeColor = _textColor;
            }
        }

        private void txtSource_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSource.Text))
            {
                txtSource.Text = SourcePlaceholder;
                txtSource.ForeColor = _placeholderColor;
                UpdateCharCount();
            }
        }

        // ── Счётчик символов ─────────────────────────────────────────

        private void txtSource_TextChanged(object sender, EventArgs e)
        {
            UpdateCharCount();
        }

        private void UpdateCharCount()
        {
            int count = (txtSource.Text == SourcePlaceholder) ? 0 : txtSource.Text.Length;
            lblCharCount.Text = $"{count} символов";
        }

        // ── Перевод ───────────────────────────────────────────────────

        private async void btnTranslate_Click(object sender, EventArgs e)
        {
            btnTranslate.Enabled = false;
            progressBar.Visible = true;
            lblStatus.Text = "Выполняется перевод...";

            try
            {
                if (string.IsNullOrWhiteSpace(txtSource.Text) || txtSource.Text == SourcePlaceholder)
                {
                    lblStatus.Text = "Введите текст для перевода!";
                    return;
                }

                if (cbTargetLang.SelectedItem == null)
                {
                    lblStatus.Text = "Выберите язык перевода!";
                    return;
                }

                string sourceText = txtSource.Text;
                string targetLang = MapLanguageToCode(cbTargetLang.SelectedItem.ToString());
                var user = _userRepository.GetById(1);
                var translation = await _translationService.TranslateText(sourceText, targetLang, user.Id);

                if (!string.IsNullOrEmpty(translation.TranslatedText))
                {
                    txtTarget.ForeColor = _textColor;
                    txtTarget.Text = translation.TranslatedText;
                }
                else
                {
                    txtTarget.ForeColor = _placeholderColor;
                    txtTarget.Text = TargetPlaceholder;
                }

                lblStatus.Text = "Перевод готов!";

                translation = _translationRepository.SaveTranslation(translation);
                SaveTranslationToFile(translation);
                lblStatus.Text = "Перевод сохранён!";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Ошибка перевода";
                MessageBox.Show($"Ошибка перевода: {ex.Message}");
            }
            finally
            {
                btnTranslate.Enabled = true;
                progressBar.Visible = false;
            }
        }

        // ── Вспомогательные ──────────────────────────────────────────

        private void SaveTranslationToFile(Translation t)
        {
            string path = Path.Combine(Application.StartupPath, "TranslatorHistory.txt");
            string line = $"{t.Id} | {t.SourceText} | {t.DetectedLanguage} | {t.TargetLanguage} | {t.TranslatedText} | {t.CreatedAt:dd.MM.yyyy HH:mm} | {t.UserId}";
            File.AppendAllText(path, line + Environment.NewLine);
        }

        private string MapLanguageToCode(string lang)
        {
            switch (lang)
            {
                case "Русский": return "RU";
                case "Английский": return "EN";
                case "Казахский": return "KK";
                case "Немецкий": return "DE";
                case "Японский": return "JA";
                case "Французский": return "FR";
                case "Испанский": return "ES";
                case "Польский": return "PL";
                default: return "EN";
            }
        }
        private void SetRoundedCorners(int radius)
        {
            IntPtr hRgn = CreateRoundRectRgn(0, 0, this.Width + 1, this.Height + 1, radius, radius);
            SetWindowRgn(this.Handle, hRgn, true);
        }

        // ── Заглушки ─────────────────────────────────────────────────

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();
        private void progressBar_Click(object sender, EventArgs e) { }
        private void lblStatus_Click(object sender, EventArgs e) { }
        private void cbTargetLang_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtTarget_TextChanged(object sender, EventArgs e) { }

        private void lblSourceLang_Click(object sender, EventArgs e)
        {

        }
    }
}