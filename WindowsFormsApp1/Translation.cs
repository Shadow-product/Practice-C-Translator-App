using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    // Модель данных для хранения информации о переводе таблицы translations в MySQL
    // get - чтение значения свойства, а set записывает значение свойства.
    // Это позволяет нам легко создавать объекты Translation и работать с их данными.
    public class Translation
    {
        public int Id { get; set; }
        public string SourceText { get; set; }
        public string DetectedLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string TranslatedText { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}