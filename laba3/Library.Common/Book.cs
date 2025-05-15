using System;

namespace Library.Common
{
    public class Book : LibraryItem
    {
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int PageCount { get; set; }
        public string Genre { get; set; }

        // Конструктор
        public Book(string title, string author, string publisher, int year, string isbn, int pageCount, string genre)
        {
            Title = title;
            Author = author;
            Publisher = publisher;
            Year = year;
            ISBN = isbn;
            PageCount = pageCount;
            Genre = genre;
        }

        // Переопределение метода GetItemType
        public override string GetItemType()
        {
            return "Книга";
        }

        // Метод для получения информации о книге
        public string GetBookInfo()
        {
            return $"Книга: {Title} (Автор: {Author}, ISBN: {ISBN}, Жанр: {Genre})";
        }
    }
} 