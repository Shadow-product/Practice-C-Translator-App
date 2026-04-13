using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(string connectionString)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {

                connection.Open();

                // Включаем поддержку внешних ключей обязательно при использовании БД SQLite,
                // иначе связь между таблицами Users и Translations работать не будет,
                // и при удалении пользователя записи переводов не будут удаляться автоматически
                using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
                {
                    pragma.ExecuteNonQuery();
                }

                // Таблица users
                // Проверка на существование таблицы users
                string createUsers = @"
                CREATE TABLE IF NOT EXISTS users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL
                );";

                using (var cmd = new SQLiteCommand(createUsers, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // Проверяем, есть ли пользователи
                using (var checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM Users", connection))
                {
                    long count = (long)checkCmd.ExecuteScalar();
                    if (count == 0)
                    {
                        // Если нет ни одного пользователя, создаём дефолтного
                        using (var insertCmd = new SQLiteCommand("INSERT INTO Users (Username) VALUES ('ApplicationUser')", connection))
                        {
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }


                // Таблица translations
                // Проверка на существование таблицы translations
                string createTranslations = @"
                CREATE TABLE IF NOT EXISTS translations (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    SourceText TEXT NOT NULL,
                    DetectedLanguage TEXT,
                    TargetLanguage TEXT NOT NULL,
                    TranslatedText TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    UserId INTEGER NOT NULL,
                    FOREIGN KEY(UserId) REFERENCES users(Id) ON DELETE CASCADE
                );";

                using (var cmd = new SQLiteCommand(createTranslations, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}