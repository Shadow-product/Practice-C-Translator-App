using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
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
