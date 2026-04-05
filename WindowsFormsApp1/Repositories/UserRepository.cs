using System;
using MySql.Data.MySqlClient; // работа с MySQL базой данных
using System.Windows.Forms;

using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Проверка на подключение к базе данных через обработчик ошибок
        public bool CheckDbConnection()
        {
            using (var connection = new MySqlConnection(_connectionString))
                try
                {
                    connection.Open();
                    MessageBox.Show("✅ Подключение к MySQL успешно!");
                    return true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Ошибка подключения к MySQL: " + ex.Message);
                    return false;
                }
        }
    }
}
