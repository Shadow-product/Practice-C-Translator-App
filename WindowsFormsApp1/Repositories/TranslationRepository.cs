    using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Transactions;
using System.Windows.Forms;

using WindowsFormsApp1.Models; // собственные модели: User, Translation

namespace WindowsFormsApp1.Repositories
{
    public class TranslationRepository
    {
        private readonly string _connectionString;

        public TranslationRepository(string connectionString)
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
                    MessageBox.Show($"Ошибка подключения к SQLite: {ex.Message}");
                    return false;
                }
        }

        // Сохранение перевода в базе данных с проверкой существования пользователя 
        // Create (создание) для переводов 
        public void SaveTranslation(Translation t)
        {
            // Проверка корректности UserId
            if (t.UserId <= 0)
                throw new ArgumentException("UserId должен быть задан и больше 0");

            // Создаём репозиторий пользователей
            var userRepository = new UserRepository(_connectionString);

            // Сначала проверяем пользователя
            var user = userRepository.GetById(t.UserId);
            if (user == null)
            {
                throw new InvalidOperationException($"Пользователь с id={t.UserId} не найден.");
            }

            // Если пользователь существует — сохраняем перевод
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var cmd = new SQLiteCommand(
                    "INSERT INTO Translations (SourceText, DetectedLanguage, TargetLanguage, " +
                    "TranslatedText, CreatedAt, UserId) " +
                    "VALUES (@s,@d,@t,@tr,@c,@u)", connection);

                cmd.Parameters.AddWithValue("@s", t.SourceText);
                cmd.Parameters.AddWithValue("@d", t.DetectedLanguage);
                cmd.Parameters.AddWithValue("@t", t.TargetLanguage);
                cmd.Parameters.AddWithValue("@tr", t.TranslatedText);
                cmd.Parameters.AddWithValue("@c", t.CreatedAt);
                cmd.Parameters.AddWithValue("@u", t.UserId);

                cmd.ExecuteNonQuery();
            }
        }

        // Read (чтение одного перевода по Id)
        public Translation GetById(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(
                    "SELECT Id, SourceText, DetectedLanguage, TargetLanguage, TranslatedText, CreatedAt, UserId " +
                    "FROM Translations WHERE Id=@id", connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Translation
                            {
                                Id = reader.GetInt32(0),
                                SourceText = reader.GetString(1),
                                DetectedLanguage = reader.IsDBNull(2) ? null : reader.GetString(2),
                                TargetLanguage = reader.GetString(3),
                                TranslatedText = reader.IsDBNull(4) ? null : reader.GetString(4),
                                CreatedAt = reader.GetDateTime(5),
                                UserId = reader.GetInt32(6)
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Read (получить все переводы пользователя)
        public List<Translation> GetByUserId(int userId)
        {
            var list = new List<Translation>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(
                    "SELECT Id, SourceText, DetectedLanguage, TargetLanguage, TranslatedText, CreatedAt " +
                    "FROM Translations WHERE UserId=@uid", connection))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Translation
                            {
                                Id = reader.GetInt32(0),
                                SourceText = reader.GetString(1),
                                DetectedLanguage = reader.IsDBNull(2) ? null : reader.GetString(2),
                                TargetLanguage = reader.GetString(3),
                                TranslatedText = reader.IsDBNull(4) ? null : reader.GetString(4),
                                CreatedAt = reader.GetDateTime(5),
                                UserId = userId
                            });
                        }
                    }
                }
            }
            return list;
        }

        // Update (обновление перевода)
        public void UpdateTranslation(Translation t)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(
                    "UPDATE Translations SET SourceText=@s, DetectedLanguage=@d, TargetLanguage=@t, " +
                    "TranslatedText=@tr, CreatedAt=@c WHERE Id=@id", connection))
                {
                    cmd.Parameters.AddWithValue("@s", t.SourceText);
                    cmd.Parameters.AddWithValue("@d", t.DetectedLanguage);
                    cmd.Parameters.AddWithValue("@t", t.TargetLanguage);
                    cmd.Parameters.AddWithValue("@tr", t.TranslatedText);
                    cmd.Parameters.AddWithValue("@c", t.CreatedAt);
                    cmd.Parameters.AddWithValue("@id", t.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Delete (удаление перевода)
        public void DeleteTranslation(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand("DELETE FROM Translations WHERE Id=@id", connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}