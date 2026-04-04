using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class TranslatorApp : Form
    {
        // Создаем экземпляр твоего сервиса
        private TranslationService _translationService = new TranslationService();

        public TranslatorApp()
        {
            InitializeComponent();
        }

        private void TranslatorApp_Load(object sender, EventArgs e)
        {
            // Проверка БД при запуске
            CheckDbConnection();
        }

        private void CheckDbConnection()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["TranslatorAppDb"].ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    lblStatus.Text = "База данных: Подключено";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "База данных: Ошибка";
                MessageBox.Show("Ошибка подключения к MySQL: " + ex.Message);
            }
        }

        // Обработчик кнопки перевода
        private async void btnTranslate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSource.Text)) return;

            btnTranslate.Enabled = false;
            progressBar.Visible = true;
            lblStatus.Text = "Перевод...";

            try
            {
                // Определяем целевой язык (например, из ComboBox или просто "ru")
                string targetLang = cbTargetLang.SelectedItem?.ToString() == "Английский" ? "en" : "ru";

                // ВЫЗОВ ТВОЕГО СЕРВИСА
                string result = await _translationService.TranslateText(txtSource.Text, targetLang);

                txtTarget.Text = result;
                lblStatus.Text = "Готово";

                // ТУТ МОЖНО ДОБАВИТЬ СОХРАНЕНИЕ В MYSQL
                // SaveToHistory(txtSource.Text, result, targetLang);
            }
            catch (Exception ex)
            {
                MessageBox.Add($"Ошибка перевода: {ex.Message}");
                lblStatus.Text = "Ошибка";
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