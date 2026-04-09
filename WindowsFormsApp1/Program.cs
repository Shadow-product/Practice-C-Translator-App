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
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string connectionString = ConfigurationManager.ConnectionStrings["TranslatorAppDb"].ConnectionString;
            DatabaseInitializer.Initialize(connectionString);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Вставляется запись - (id автоинкремент автоматически ставится первичный ключ)
                string insertSql = "INSERT INTO users (username) VALUES (@username)";
                using (var cmd = new SQLiteCommand(insertSql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "Тест");
                    cmd.ExecuteNonQuery();
                }

                // Чтение всех записей из таблицы users пример
                string selectSql = "SELECT Id, Username FROM users";
                using (var cmd = new SQLiteCommand(selectSql, connection))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["id"]}: {reader["username"]}");
                    }
                }
            }

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