using System;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using System.Collections.Generic;
using System.Transactions;
using System.Windows.Forms;

namespace WindowsFormsApp1
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

        public void SaveTranslation(Translation t)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var cmd = new MySqlCommand(
                    "INSERT INTO translations (source_text, detected_language, target_language," +
                    " translated_text, created_at, user_id)" +
                    " VALUES (@s,@d,@t,@tr,@c,@u)",
                    connection);

                cmd.Parameters.AddWithValue("@s", t.SourceText);
                cmd.Parameters.AddWithValue("@d", t.DetectedLanguage);
                cmd.Parameters.AddWithValue("@t", t.TargetLanguage);
                cmd.Parameters.AddWithValue("@tr", t.TranslatedText);
                cmd.Parameters.AddWithValue("@c", t.CreatedAt);
                cmd.Parameters.AddWithValue("@u", t.UserId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}