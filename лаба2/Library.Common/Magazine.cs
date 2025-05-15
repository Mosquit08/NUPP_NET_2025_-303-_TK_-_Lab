using System;

namespace Library.Common
{
    public class Magazine : LibraryItem
    {
        public string ISSN { get; set; }
        public int IssueNumber { get; set; }
        public string Category { get; set; }
        public string Editor { get; set; }

        // Конструктор
        public Magazine(string title, string publisher, int year, string issn, int issueNumber, string category, string editor)
        {
            Title = title;
            Publisher = publisher;
            Year = year;
            ISSN = issn;
            IssueNumber = issueNumber;
            Category = category;
            Editor = editor;
        }

        // Переопределение метода GetItemType
        public override string GetItemType()
        {
            return "Журнал";
        }

        // Метод для получения информации о журнале
        public string GetMagazineInfo()
        {
            return $"Журнал: {Title} (Номер: {IssueNumber}, Категория: {Category}, Редактор: {Editor})";
        }
    }
} 