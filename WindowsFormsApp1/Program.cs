using System;
using System.Configuration;
using System.Windows.Forms;
using System.Data.SQLite;
using WindowsFormsApp1;

using WindowsFormsApp1.Forms; // форма приложения TranslatorApp
using WindowsFormsApp1.Data; // инициализация базы данных

namespace WindowsFormsApp1
{

    // Пример подключения к MySQL, что она работает и сохраняются данные в базу данных
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Инициализация репозитория с обязательным параметром подключения для работы с базой данных
            string connectionString = ConfigurationManager.ConnectionStrings["TranslatorAppDb"].ConnectionString;
            DatabaseInitializer.Initialize(connectionString);

            // Запуск формы приложения с обработчиком ошибок
            try
            {
                Application.Run(new TranslatorApp());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}