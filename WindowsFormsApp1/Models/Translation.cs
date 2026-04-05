using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindowsFormsApp1
{
    // Модель данных для хранения информации о переводе таблицы translations в MySQL
    // get - чтение значения свойства, а set записывает значение свойства
    // Это позволяет нам легко создавать объекты Translation и работать с их данными
    // Навигационное свойство — это обычное свойство класса, которое указывает на связанную сущность.
    // Оно не хранится в таблице напрямую, а используется ORM (Entity Framework) для того,
    // чтобы подтягивать связанные данные
    [Table("translations")]
    public class Translation
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("source_text")]
        public string SourceText { get; set; }

        [Column("detected_language")]
        public string DetectedLanguage { get; set; }

        [Column("target_language")]
        public string TargetLanguage { get; set; }

        [Column("translated_text")]
        public string TranslatedText { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        // Навигационное свойство: перевод связан с одним пользователем
        public User Owner{ get; set; }
    }
}