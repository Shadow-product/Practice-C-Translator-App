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

        // Подключение к UserRepository и текущему пользователю для сохранения информации о том, кто сделал перевод
        // Пока не работает необходимо добавить в дизайн формы ComboBox для выбора пользователя и загрузку
        // пользователей из БД при загрузке формы
        // private UserRepository _userRepository;
        // private User _currentUser;


        public TranslatorApp()
        {
            InitializeComponent();

            // Инициализация репозитория с обязательным параметром подключения для работы с базой данных
            string connectionString = ConfigurationManager.ConnectionStrings["TranslatorAppDb"].ConnectionString;
            _translationRepository = new TranslationRepository(connectionString);

            // Инициализация сервиса перевода для работы с API и получения перевода текста
            string apiKey = ConfigurationManager.AppSettings["ApiKey"];
            _translationService = new TranslationService();
        }

        private void TranslatorApp_Load(object sender, EventArgs e)
        {
            try
            {
                // Чтение строки подключения из файла App.config TranslatorAppDb имя строки подключения
                var connectionString = ConfigurationManager.ConnectionStrings["TranslatorAppDb"].ConnectionString;
                _translationRepository = new TranslationRepository(connectionString);
                //_userRepository = new UserRepository(connectionString);

                // Проверка подключения к базе данных 
                if (_translationRepository.CheckDbConnection())
                    //&& _userRepository.CheckDbConnection())
                    lblStatus.Text = "База данных: Подключено";
                else
                    lblStatus.Text = "База данных: Ошибка";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "База данных: Ошибка";
                MessageBox.Show($"❌ Ошибка подключения: {ex.Message}");
            }
        }

        //private void LoadUsers()
        //{
        //    cbUsers.DataSource = _userRepository.GetAllUsers();
        //    cbUsers.DisplayMember = "Username";
        //    cbUsers.ValueMember = "Id";
        //}

        //private void cbUsers_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    _currentUser = (User)cbUsers.SelectedItem;
        //}

        // Обработчик кнопки перевода
        private async void btnTranslate_Click(object sender, EventArgs e)
        {
            //if (_currentUser == null)
            //{
            //    MessageBox.Show("Выберите пользователя!");
            //    return;
            //}

            btnTranslate.Enabled = false;
            progressBar.Visible = true;
            lblStatus.Text = "Выполняется перевод...";
            string text = txtSource.Text;
            string lang = cbTargetLang.SelectedItem.ToString();

            string translated = await _translationService.TranslateText(text, lang);
            txtTarget.Text = translated;
            try
            {

                // Проверка на пустую строку в текстовом поле перевода через длину строки
                if (txtSource.Text == null || txtSource.Text.Trim().Length == 0)
                {
                    lblStatus.Text = "Введите текст для перевода!";
                    return;
                }

                string sourceText = txtSource.Text;
                // Определение целевого языка (например, из ComboBox или просто "ru")
                string targetLang = cbTargetLang.SelectedItem?.ToString() == "Английский" ? "en" : "ru";
                if (cbTargetLang.SelectedItem == null)
                {
                    lblStatus.Text = "Выберите язык перевода!";
                    return;
                }

                // вызов сервиса перевода TranslationService файл для получения перевода
                string result = await _translationService.TranslateText(txtSource.Text, targetLang);
                txtTarget.Text = result != null ? result : "Перевод недоступен";
                lblStatus.Text = "Перевод готов!";

                // Сохранение в БД через репозиторий класса TranslationRepository и модели Translation
                var translation = new Translation
                {
                    SourceText = txtSource.Text,
                    TargetLanguage = targetLang,
                    TranslatedText = result,
                    DetectedLanguage = "ru", // можно доработать автоопределение
                    CreatedAt = DateTime.Now,
                    //UserId = _currentUser.Id // Пока не работает необходимо добавить в дизайн формы ComboBox для выбора пользователя и загрузку
                };

                _translationRepository.SaveTranslation(translation);
                lblStatus.Text = "Перевод сохранён!";

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Ошибка перевода";
                MessageBox.Show($" ❌ Ошибка перевода: {ex.Message}");
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