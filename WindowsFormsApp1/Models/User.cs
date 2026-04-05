using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindowsFormsApp1
{
    // Модель данных для хранения пользователей таблицы users в MySQL
    // get - чтение значения свойства, а set записывает значение свойства
    // Это позволяет нам легко создавать объекты User и работать с их данными
    // Навигационное свойство — это обычное свойство класса, которое указывает на связанную сущность.
    // Оно не хранится в таблице напрямую, а используется ORM (Entity Framework) для того,
    // чтобы подтягивать связанные данные
    [Table("users")]
    public class User
    {
        [Key] // первичный ключ 
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        // Навигационное свойство: один пользователь -> много переводов
        public ICollection<Translation> Translations { get; set; }
    }
}
