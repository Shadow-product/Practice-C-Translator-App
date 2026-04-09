using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
            using (var connection = new SQLiteConnection(_connectionString))
                try
                {
                    connection.Open();
                    MessageBox.Show("Подключение к SQLite успешно!");
                    return true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения к SQLite. Проверьте путь к базе и наличие sqlite3.dll.\n " + ex.Message);
                    return false;
                }
        }

        // Проверка на CRUD операции в базе данных
        // Create (создание) пользователя в базе данных
        public void AddUser(string username)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand("INSERT INTO Users (Username) VALUES (@name)", connection))
                {
                    cmd.Parameters.AddWithValue("@name", username);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Read (чтение одного пользователя) пользователя по имени из базы данных
        public User GetById(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand("SELECT Id, Username FROM Users WHERE Id=@id", connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1)
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Read (чтение всех пользователей) всех пользователей из базы данных
        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT Id, Username FROM Users", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1)
                        });
                    }
                }
            }
            return users;
        }

        // Update (обновление) имени пользователя в базе данных
        public void UpdateUser(User user)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("UPDATE Users SET Username=@name WHERE Id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@name", user.Username);
                    cmd.Parameters.AddWithValue("@id", user.Id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException($"Пользователь с Id={user.Id} не найден.");
                    }
                }
            }
        }

        // Delete (удаление) пользователя из базы данных по Id
        public void DeleteUser(int id)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("DELETE FROM Users WHERE Id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}