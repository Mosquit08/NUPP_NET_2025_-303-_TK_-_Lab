using System;
using System.Linq;
using Library.Common;

namespace Library.Console
{
    class Program
    {
        private static CrudService<Book> bookService = new CrudService<Book>();
        private static CrudService<Reader> readerService = new CrudService<Reader>();
        private static CrudService<BorrowingRecord> borrowingService = new CrudService<BorrowingRecord>();

        static void Main(string[] args)
        {
            LoadData();
            ShowMainMenu();
        }

        static void LoadData()
        {
            try
            {
                bookService.Load("books.json");
                readerService.Load("readers.json");
                borrowingService.Load("borrowings.json");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        static void SaveData()
        {
            try
            {
                bookService.Save("books.json");
                readerService.Save("readers.json");
                borrowingService.Save("borrowings.json");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        static void ShowMainMenu()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== Библиотечная система ===");
                System.Console.WriteLine("1. Управление книгами");
                System.Console.WriteLine("2. Управление читателями");
                System.Console.WriteLine("3. Управление выдачами");
                System.Console.WriteLine("0. Выход");
                System.Console.Write("\nВыберите действие: ");

                var choice = System.Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowBookMenu();
                        break;
                    case "2":
                        ShowReaderMenu();
                        break;
                    case "3":
                        ShowBorrowingMenu();
                        break;
                    case "0":
                        SaveData();
                        return;
                    default:
                        System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowBookMenu()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== Управление книгами ===");
                System.Console.WriteLine("1. Показать все книги");
                System.Console.WriteLine("2. Добавить книгу");
                System.Console.WriteLine("3. Редактировать книгу");
                System.Console.WriteLine("4. Удалить книгу");
                System.Console.WriteLine("0. Назад");
                System.Console.Write("\nВыберите действие: ");

                var choice = System.Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowAllBooks();
                        break;
                    case "2":
                        AddBook();
                        break;
                    case "3":
                        EditBook();
                        break;
                    case "4":
                        DeleteBook();
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowReaderMenu()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== Управление читателями ===");
                System.Console.WriteLine("1. Показать всех читателей");
                System.Console.WriteLine("2. Добавить читателя");
                System.Console.WriteLine("3. Редактировать читателя");
                System.Console.WriteLine("4. Удалить читателя");
                System.Console.WriteLine("0. Назад");
                System.Console.Write("\nВыберите действие: ");

                var choice = System.Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowAllReaders();
                        break;
                    case "2":
                        AddReader();
                        break;
                    case "3":
                        EditReader();
                        break;
                    case "4":
                        DeleteReader();
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowBorrowingMenu()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== Управление выдачами ===");
                System.Console.WriteLine("1. Показать все выдачи");
                System.Console.WriteLine("2. Выдать книгу");
                System.Console.WriteLine("3. Вернуть книгу");
                System.Console.WriteLine("0. Назад");
                System.Console.Write("\nВыберите действие: ");

                var choice = System.Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowAllBorrowings();
                        break;
                    case "2":
                        BorrowBook();
                        break;
                    case "3":
                        ReturnBook();
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowAllBooks()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Список всех книг ===");
            foreach (var book in bookService.ReadAll())
            {
                System.Console.WriteLine(book.GetBookInfo());
            }
            System.Console.WriteLine("\nНажмите любую клавишу для возврата...");
            System.Console.ReadKey();
        }

        static void AddBook()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Добавление новой книги ===");
            
            System.Console.Write("Название: ");
            var title = System.Console.ReadLine();
            
            System.Console.Write("Автор: ");
            var author = System.Console.ReadLine();
            
            System.Console.Write("Издательство: ");
            var publisher = System.Console.ReadLine();
            
            System.Console.Write("Год издания: ");
            if (!int.TryParse(System.Console.ReadLine(), out int year))
            {
                System.Console.WriteLine("Неверный формат года. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }
            
            System.Console.Write("ISBN: ");
            var isbn = System.Console.ReadLine();
            
            System.Console.Write("Количество страниц: ");
            if (!int.TryParse(System.Console.ReadLine(), out int pages))
            {
                System.Console.WriteLine("Неверный формат количества страниц. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }
            
            System.Console.Write("Жанр: ");
            var genre = System.Console.ReadLine();

            var book = new Book(title, author, publisher, year, isbn, pages, genre);
            bookService.Create(book);
            
            System.Console.WriteLine("Книга успешно добавлена. Нажмите любую клавишу...");
            System.Console.ReadKey();
        }

        static void EditBook()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Редактирование книги ===");
            
            System.Console.Write("Введите ID книги: ");
            if (!Guid.TryParse(System.Console.ReadLine(), out Guid id))
            {
                System.Console.WriteLine("Неверный формат ID. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            var book = bookService.Read(id);
            if (book == null)
            {
                System.Console.WriteLine("Книга не найдена. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            System.Console.WriteLine($"Текущая информация: {book.GetBookInfo()}");
            System.Console.WriteLine("\nВведите новые данные (оставьте пустым для сохранения текущего значения):");
            
            System.Console.Write("Название: ");
            var title = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title)) book.Title = title;
            
            System.Console.Write("Автор: ");
            var author = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(author)) book.Author = author;
            
            System.Console.Write("Издательство: ");
            var publisher = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(publisher)) book.Publisher = publisher;
            
            System.Console.Write("Год издания: ");
            var yearStr = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(yearStr) && int.TryParse(yearStr, out int year))
            {
                book.Year = year;
            }
            
            System.Console.Write("ISBN: ");
            var isbn = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(isbn)) book.ISBN = isbn;
            
            System.Console.Write("Количество страниц: ");
            var pagesStr = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(pagesStr) && int.TryParse(pagesStr, out int pages))
            {
                book.PageCount = pages;
            }
            
            System.Console.Write("Жанр: ");
            var genre = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(genre)) book.Genre = genre;

            bookService.Update(book);
            System.Console.WriteLine("Книга успешно обновлена. Нажмите любую клавишу...");
            System.Console.ReadKey();
        }

        static void DeleteBook()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Удаление книги ===");
            
            System.Console.Write("Введите ID книги: ");
            if (!Guid.TryParse(System.Console.ReadLine(), out Guid id))
            {
                System.Console.WriteLine("Неверный формат ID. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            var book = bookService.Read(id);
            if (book == null)
            {
                System.Console.WriteLine("Книга не найдена. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            System.Console.WriteLine($"Вы уверены, что хотите удалить книгу: {book.GetBookInfo()}? (y/n)");
            if (System.Console.ReadLine().ToLower() == "y")
            {
                bookService.Remove(book);
                System.Console.WriteLine("Книга успешно удалена. Нажмите любую клавишу...");
            }
            else
            {
                System.Console.WriteLine("Удаление отменено. Нажмите любую клавишу...");
            }
            System.Console.ReadKey();
        }

        static void ShowAllReaders()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Список всех читателей ===");
            foreach (var reader in readerService.ReadAll())
            {
                System.Console.WriteLine($"ID: {reader.Id}");
                System.Console.WriteLine($"ФИО: {reader.GetFullName()}");
                System.Console.WriteLine($"Email: {reader.Email}");
                System.Console.WriteLine($"Телефон: {reader.PhoneNumber}");
                System.Console.WriteLine("------------------------");
            }
            System.Console.WriteLine("\nНажмите любую клавишу для возврата...");
            System.Console.ReadKey();
        }

        static void AddReader()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Добавление нового читателя ===");
            
            System.Console.Write("Имя: ");
            var firstName = System.Console.ReadLine();
            
            System.Console.Write("Фамилия: ");
            var lastName = System.Console.ReadLine();
            
            System.Console.Write("Email: ");
            var email = System.Console.ReadLine();
            
            System.Console.Write("Телефон: ");
            var phone = System.Console.ReadLine();

            var reader = new Reader(firstName, lastName, email, phone);
            readerService.Create(reader);
            
            System.Console.WriteLine("Читатель успешно добавлен. Нажмите любую клавишу...");
            System.Console.ReadKey();
        }

        static void EditReader()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Редактирование читателя ===");
            
            System.Console.Write("Введите ID читателя: ");
            if (!Guid.TryParse(System.Console.ReadLine(), out Guid id))
            {
                System.Console.WriteLine("Неверный формат ID. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            var reader = readerService.Read(id);
            if (reader == null)
            {
                System.Console.WriteLine("Читатель не найден. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            System.Console.WriteLine($"Текущая информация: {reader.GetFullName()}, Email: {reader.Email}, Телефон: {reader.PhoneNumber}");
            System.Console.WriteLine("\nВведите новые данные (оставьте пустым для сохранения текущего значения):");
            
            System.Console.Write("Имя: ");
            var firstName = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(firstName)) reader.FirstName = firstName;
            
            System.Console.Write("Фамилия: ");
            var lastName = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(lastName)) reader.LastName = lastName;
            
            System.Console.Write("Email: ");
            var email = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email)) reader.Email = email;
            
            System.Console.Write("Телефон: ");
            var phone = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(phone)) reader.PhoneNumber = phone;

            readerService.Update(reader);
            System.Console.WriteLine("Читатель успешно обновлен. Нажмите любую клавишу...");
            System.Console.ReadKey();
        }

        static void DeleteReader()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Удаление читателя ===");
            
            System.Console.Write("Введите ID читателя: ");
            if (!Guid.TryParse(System.Console.ReadLine(), out Guid id))
            {
                System.Console.WriteLine("Неверный формат ID. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            var reader = readerService.Read(id);
            if (reader == null)
            {
                System.Console.WriteLine("Читатель не найден. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            System.Console.WriteLine($"Вы уверены, что хотите удалить читателя: {reader.GetFullName()}? (y/n)");
            if (System.Console.ReadLine().ToLower() == "y")
            {
                readerService.Remove(reader);
                System.Console.WriteLine("Читатель успешно удален. Нажмите любую клавишу...");
            }
            else
            {
                System.Console.WriteLine("Удаление отменено. Нажмите любую клавишу...");
            }
            System.Console.ReadKey();
        }

        static void ShowAllBorrowings()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Список всех выдач ===");
            foreach (var borrowing in borrowingService.ReadAll())
            {
                var reader = readerService.Read(borrowing.ReaderId);
                var book = bookService.Read(borrowing.ItemId);
                System.Console.WriteLine($"ID: {borrowing.Id}");
                System.Console.WriteLine($"Читатель: {reader?.GetFullName() ?? "Не найден"}");
                System.Console.WriteLine($"Книга: {book?.Title ?? "Не найдена"}");
                System.Console.WriteLine($"Дата выдачи: {borrowing.BorrowDate}");
                System.Console.WriteLine($"Статус: {(borrowing.IsReturned ? "Возвращена" : "На руках")}");
                if (borrowing.IsReturned)
                {
                    System.Console.WriteLine($"Дата возврата: {borrowing.ReturnDate}");
                }
                System.Console.WriteLine("------------------------");
            }
            System.Console.WriteLine("\nНажмите любую клавишу для возврата...");
            System.Console.ReadKey();
        }

        static void BorrowBook()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Выдача книги ===");
            
            System.Console.Write("Введите ID читателя: ");
            if (!Guid.TryParse(System.Console.ReadLine(), out Guid readerId))
            {
                System.Console.WriteLine("Неверный формат ID читателя. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            var reader = readerService.Read(readerId);
            if (reader == null)
            {
                System.Console.WriteLine("Читатель не найден. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            System.Console.Write("Введите ID книги: ");
            if (!Guid.TryParse(System.Console.ReadLine(), out Guid bookId))
            {
                System.Console.WriteLine("Неверный формат ID книги. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            var book = bookService.Read(bookId);
            if (book == null)
            {
                System.Console.WriteLine("Книга не найдена. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            if (!book.IsAvailable)
            {
                System.Console.WriteLine("Книга уже выдана. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            var borrowing = new BorrowingRecord(readerId, bookId);
            borrowingService.Create(borrowing);
            book.ChangeAvailability(false);
            bookService.Update(book);
            reader.AddBorrowedItem(bookId);
            readerService.Update(reader);

            System.Console.WriteLine("Книга успешно выдана. Нажмите любую клавишу...");
            System.Console.ReadKey();
        }

        static void ReturnBook()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Возврат книги ===");
            
            System.Console.Write("Введите ID записи выдачи: ");
            if (!Guid.TryParse(System.Console.ReadLine(), out Guid id))
            {
                System.Console.WriteLine("Неверный формат ID. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            var borrowing = borrowingService.Read(id);
            if (borrowing == null)
            {
                System.Console.WriteLine("Запись выдачи не найдена. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            if (borrowing.IsReturned)
            {
                System.Console.WriteLine("Книга уже возвращена. Нажмите любую клавишу...");
                System.Console.ReadKey();
                return;
            }

            borrowing.Return();
            borrowingService.Update(borrowing);

            var book = bookService.Read(borrowing.ItemId);
            book.ChangeAvailability(true);
            bookService.Update(book);

            var reader = readerService.Read(borrowing.ReaderId);
            reader.ReturnItem(borrowing.ItemId);
            readerService.Update(reader);

            System.Console.WriteLine("Книга успешно возвращена. Нажмите любую клавишу...");
            System.Console.ReadKey();
        }
    }
}
