using System;
using System.Configuration; // конфигурации Entity Framework
using System.Windows.Forms; // элементы WinForms: Form, Button, Label
using WindowsFormsApp1;
using System.Threading.Tasks; // для асинхронной работы

using WindowsFormsApp1.Models; // собственные модели: User, Translation
using WindowsFormsApp1.Services;  // бизнес-логика и работа с API (TranslationService)
using WindowsFormsApp1.Repositories; // классы доступа к данным (UserRepository, TranslationRepository)

namespace WindowsFormsApp1.Forms
{
    public partial class TranslatorApp : Form
    {
        // Поля для сервисов и репозиториев
        private TranslationService _translationService;
        private TranslationRepository _translationRepository;

        public TranslatorApp()
        {
            InitializeComponent();

            // Инициализация репозитория с обязательным параметром подключения для работы с базой данных
            string connectionString = ConfigurationManager.ConnectionStrings["TranslatorAppDb"].ConnectionString;
            _translationRepository = new TranslationRepository(connectionString);

            // Инициализация сервиса перевода для работы с API и получения перевода текста
            string apiKey = ConfigurationManager.AppSettings["ApiKey"];
            _translationService = new TranslationService(apiKey);
        }

        private void TranslatorApp_Load(object sender, EventArgs e)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["TranslatorAppDb"].ConnectionString;
                _translationRepository = new TranslationRepository(connectionString);

                if (_translationRepository.CheckDbConnection())
                    lblStatus.Text = "База данных: Подключено";
                else
                    lblStatus.Text = "База данных: Ошибка";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "База данных: Ошибка";
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
            }
        }

        private async void btnTranslate_Click(object sender, EventArgs e)
        {
            btnTranslate.Enabled = false;
            progressBar.Visible = true;
            lblStatus.Text = "Выполняется перевод...";

            try
            {
                if (string.IsNullOrWhiteSpace(txtSource.Text))
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
                string targetLang = cbTargetLang.SelectedItem.ToString() == "Английский" ? "en" : "ru";

                // Вызов API
                string result = await _translationService.TranslateText(sourceText, targetLang);

                txtTarget.Text = result != null && result != "" ? result : "Перевод недоступен";
                lblStatus.Text = "Перевод готов!";

                // Сохранение в БД
                var translation = new Translation
                {
                    SourceText = sourceText,
                    TargetLanguage = targetLang,
                    TranslatedText = result,
                    DetectedLanguage = "ru", // можно доработать автоопределение
                    CreatedAt = DateTime.Now,
                    UserId = 4 // временно фиксированный пользователь
                };

                _translationRepository.SaveTranslation(translation);
                lblStatus.Text = "Перевод сохранён!";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Ошибка перевода";
                MessageBox.Show($"Ошибка перевода: {ex}");
            }
            finally
            {
                btnTranslate.Enabled = true;
                progressBar.Visible = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
