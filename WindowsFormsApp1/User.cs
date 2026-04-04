using System;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    // Модель данных для хранения пользователей таблицы users в MySQL
    // get - чтение значения свойства, а set записывает значение свойства.
    // Это позволяет нам легко создавать объекты User и работать с их данными.
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
}
