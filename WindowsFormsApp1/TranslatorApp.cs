using System;
using System.Configuration;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class TranslatorApp : Form
    {
        public TranslatorApp()
        {
            InitializeComponent();
        }

        // Событие загрузки формы для проверки подключения к базе данных MySQL
        // с использованием  (try catch) обработчика исключений (ошибок)
        private void TranslatorApp_Load(object sender, EventArgs e)
        {
            // Чтение строки подключения из файла App.config TranslatorAppDb имя строки подключения
            string connectionString = ConfigurationManager.ConnectionStrings["TranslatorAppDb"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("✅ Подключение к MySQL успешно!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Ошибка подключения: " + ex.Message);
                }
            }
        }
    }
}