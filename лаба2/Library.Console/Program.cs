using System;
using System.Linq;
using Library.Common;

namespace Library.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем сервисы для работы с разными типами данных
            var bookService = new CrudService<Book>();
            var readerService = new CrudService<Reader>();
            var borrowingService = new CrudService<BorrowingRecord>();

            // Создаем тестовые данные
            var book1 = new Book("Война и мир", "Лев Толстой", "Литопис", 1869, "978-5-389-06256-6", 1225, "Роман");
            var book2 = new Book("Катерина", "Тарас Шевченкко", "Украинский вестник", 1866, "978-5-389-06257-3", 671, "Роман");

            var reader1 = new Reader("Иван", "Иванов", "ivan@example.com", "+38-999-123-45-67");
            var reader2 = new Reader("Петр", "Петров", "petr@example.com", "+38-999-765-43-21");

            // Демонстрация Func делегата
            System.Console.WriteLine("\nДемонстрация информации о читателе:");
            System.Console.WriteLine(reader1.GetReaderInfo());

            // Добавляем книги в сервис
            bookService.Create(book1);
            bookService.Create(book2);

            // Добавляем читателей в сервис
            readerService.Create(reader1);
            readerService.Create(reader2);

            // Создаем запись о выдаче книги
            var borrowing = new BorrowingRecord(reader1.Id, book1.Id);
            borrowingService.Create(borrowing);

            // Демонстрация проверки валидности выдачи
            System.Console.WriteLine("\nДемонстрация проверки валидности выдачи:");
            System.Console.WriteLine($"Валидность выдачи: {borrowing.ValidateBorrowing()}");

            // Подписываемся на события читателя
            reader1.BookBorrowed += (sender, message) => System.Console.WriteLine($"Событие: {message}");
            reader1.BookReturned += (sender, message) => System.Console.WriteLine($"Событие: {message}");

            // Подписываемся на событие изменения статуса книги
            book1.StatusChanged += (sender, e) => 
                System.Console.WriteLine($"EventHandler: Статус книги {e.ItemTitle} изменен на {(e.IsAvailable ? "доступна" : "недоступна")}");

            // Добавляем книгу в список взятых читателем
            reader1.AddBorrowedItem(book1.Id);

            // Выводим информацию о всех книгах
            System.Console.WriteLine("\nСписок всех книг:");
            foreach (var book in bookService.ReadAll())
            {
                System.Console.WriteLine(book.GetBookInfo());
            }

            // Выводим информацию о всех читателях
            System.Console.WriteLine("\nСписок всех читателей:");
            foreach (var reader in readerService.ReadAll())
            {
                System.Console.WriteLine($"Читатель: {reader.GetFullName()}, Email: {reader.Email}");
            }

            // Выводим информацию о записях выдачи
            System.Console.WriteLine("\nСписок всех записей выдачи:");
            foreach (var record in borrowingService.ReadAll())
            {
                System.Console.WriteLine(record.GetRecordInfo());
            }

            // Демонстрация обновления данных
            book1.ChangeAvailability(false);
            bookService.Update(book1);

            // Демонстрация возврата книги
            borrowing.Return();
            borrowingService.Update(borrowing);
            reader1.ReturnItem(book1.Id);

            // Сохраняем данные в файлы
            bookService.Save("books.json");
            readerService.Save("readers.json");
            borrowingService.Save("borrowings.json");

            // Загружаем данные из файлов
            var newBookService = new CrudService<Book>();
            newBookService.Load("books.json");

            System.Console.WriteLine("\nДанные после загрузки из файла:");
            foreach (var book in newBookService.ReadAll())
            {
                System.Console.WriteLine(book.GetBookInfo());
            }

            System.Console.WriteLine("\nНажмите любую клавишу для выхода...");
            System.Console.ReadKey();
        }
    }
}
