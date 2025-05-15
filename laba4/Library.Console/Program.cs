using System;
using System.Threading.Tasks;
using System.Linq;
using Library.Infrastructure;
using Library.Infrastructure.Models;
using Library.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Library.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var settings = new DatabaseSettings
            {
                Server = "localhost",
                Database = "library",
                User = "root",
                Password = "",
                Port = 3306
            };

            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            var connectionString = $"Server={settings.Server};Port={settings.Port};Database={settings.Database};User={settings.User};Password={settings.Password};";
            
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            
            using var context = new LibraryContext(optionsBuilder.Options);

            while (true)
            {
                System.Console.WriteLine("\nВыберите операцию:");
                System.Console.WriteLine("1. Показать все данные");
                System.Console.WriteLine("2. Добавить тестовые данные");
                System.Console.WriteLine("3. Обновить запись");
                System.Console.WriteLine("4. Удалить запись");
                System.Console.WriteLine("0. Выход");

                var choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ShowAllData(context);
                        break;
                    case "2":
                        await AddTestData(context);
                        break;
                    case "3":
                        await UpdateRecord(context);
                        break;
                    case "4":
                        await DeleteRecord(context);
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        static async Task ShowAllData(LibraryContext context)
        {
            // Выводим список всех книг
            System.Console.WriteLine("\nСписок всех книг:");
            var books = await context.Books.ToListAsync();
            foreach (var b in books)
            {
                System.Console.WriteLine($"ID: {b.Id} - {b.Title} by {b.Author}");
            }

            // Выводим список всех журналов
            System.Console.WriteLine("\nСписок всех журналов:");
            var magazines = await context.Magazines.ToListAsync();
            foreach (var m in magazines)
            {
                System.Console.WriteLine($"ID: {m.Id} - {m.Title} (ISSN: {m.ISSN})");
            }

            // Выводим список всех читателей
            System.Console.WriteLine("\nСписок всех читателей:");
            var readers = await context.Readers.ToListAsync();
            foreach (var reader in readers)
            {
                System.Console.WriteLine($"ID: {reader.Id} - {reader.FirstName} {reader.LastName}");
            }

            // Выводим список всех записей выдачи
            System.Console.WriteLine("\nСписок всех записей выдачи:");
            var records = await context.BorrowingRecords.ToListAsync();
            foreach (var record in records)
            {
                System.Console.WriteLine($"ID: {record.Id} - {record.BorrowDate} - {record.ReturnDate}");
            }
        }

        static async Task AddTestData(LibraryContext context)
        {
            try
            {
                // Проверяем наличие данных перед добавлением
                if (!await context.Books.AnyAsync())
                {
                    System.Console.WriteLine("Добавление тестовых книг...");
                    var book1 = new BookModel
                    {
                        Title = "Test Book 1",
                        Author = "Test Author 1",
                        Publisher = "Test Publisher 1",
                        Year = 2024,
                        ISBN = "1234567890123",
                        Genre = "Test Genre 1",
                        PageCount = 100,
                        IsAvailable = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    var book2 = new BookModel
                    {
                        Title = "Test Book 2",
                        Author = "Test Author 2",
                        Publisher = "Test Publisher 2",
                        Year = 2023,
                        ISBN = "9876543210987",
                        Genre = "Test Genre 2",
                        PageCount = 200,
                        IsAvailable = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    context.Books.Add(book1);
                    context.Books.Add(book2);
                }

                if (!await context.Magazines.AnyAsync())
                {
                    System.Console.WriteLine("Добавление тестовых журналов...");
                    var magazine1 = new MagazineModel
                    {
                        Title = "Test Magazine 1",
                        Publisher = "Test Publisher 1",
                        Year = 2024,
                        ISSN = "1234-5678",
                        Category = "Test Category 1",
                        Editor = "Test Editor 1",
                        IsAvailable = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    var magazine2 = new MagazineModel
                    {
                        Title = "Test Magazine 2",
                        Publisher = "Test Publisher 2",
                        Year = 2023,
                        ISSN = "8765-4321",
                        Category = "Test Category 2",
                        Editor = "Test Editor 2",
                        IsAvailable = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    context.Magazines.Add(magazine1);
                    context.Magazines.Add(magazine2);
                }

                if (!await context.Readers.AnyAsync())
                {
                    System.Console.WriteLine("Добавление тестовых читателей...");
                    var reader1 = new ReaderModel
                    {
                        FirstName = "Test FirstName 1",
                        LastName = "Test LastName 1",
                        Email = "test1@example.com",
                        PhoneNumber = "1234567890",
                        RegistrationDate = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    };
                    var reader2 = new ReaderModel
                    {
                        FirstName = "Test FirstName 2",
                        LastName = "Test LastName 2",
                        Email = "test2@example.com",
                        PhoneNumber = "0987654321",
                        RegistrationDate = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    };
                    context.Readers.Add(reader1);
                    context.Readers.Add(reader2);
                }

                await context.SaveChangesAsync();
                System.Console.WriteLine("Тестовые данные добавлены успешно");

                // Добавляем записи выдачи только если их нет
                if (!await context.BorrowingRecords.AnyAsync())
                {
                    System.Console.WriteLine("Добавление тестовых записей выдачи...");
                    var reader1 = await context.Readers.FirstOrDefaultAsync();
                    var book1 = await context.Books.FirstOrDefaultAsync();
                    var magazine1 = await context.Magazines.FirstOrDefaultAsync();

                    if (reader1 != null && book1 != null)
                    {
                        var record1 = new BorrowingRecordModel
                        {
                            ReaderId = reader1.Id,
                            ItemId = book1.Id,
                            BorrowDate = DateTime.UtcNow,
                            IsReturned = false,
                            CreatedAt = DateTime.UtcNow
                        };
                        context.BorrowingRecords.Add(record1);
                    }

                    if (reader1 != null && magazine1 != null)
                    {
                        var record2 = new BorrowingRecordModel
                        {
                            ReaderId = reader1.Id,
                            ItemId = magazine1.Id,
                            BorrowDate = DateTime.UtcNow,
                            IsReturned = false,
                            CreatedAt = DateTime.UtcNow
                        };
                        context.BorrowingRecords.Add(record2);
                    }

                    await context.SaveChangesAsync();
                    System.Console.WriteLine("Тестовые записи выдачи добавлены успешно");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при добавлении тестовых данных: {ex.Message}");
                System.Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        static async Task UpdateRecord(LibraryContext context)
        {
            System.Console.WriteLine("\nВыберите тип записи для обновления:");
            System.Console.WriteLine("1. Книга");
            System.Console.WriteLine("2. Журнал");
            System.Console.WriteLine("3. Читатель");
            System.Console.WriteLine("4. Запись выдачи");

            var choice = System.Console.ReadLine();
            System.Console.Write("Введите ID записи для обновления: ");
            if (!Guid.TryParse(System.Console.ReadLine(), out Guid id))
            {
                System.Console.WriteLine("Неверный формат ID");
                return;
            }

            try
            {
                switch (choice)
                {
                    case "1":
                        var book = await context.Books.FindAsync(id);
                        if (book != null)
                        {
                            System.Console.Write("Новое название: ");
                            book.Title = System.Console.ReadLine();
                            System.Console.Write("Новый автор: ");
                            book.Author = System.Console.ReadLine();
                            await context.SaveChangesAsync();
                            System.Console.WriteLine("Книга успешно обновлена");
                        }
                        else
                        {
                            System.Console.WriteLine("Книга не найдена");
                        }
                        break;

                    case "2":
                        var magazine = await context.Magazines.FindAsync(id);
                        if (magazine != null)
                        {
                            System.Console.Write("Новое название: ");
                            magazine.Title = System.Console.ReadLine();
                            System.Console.Write("Новый редактор: ");
                            magazine.Editor = System.Console.ReadLine();
                            await context.SaveChangesAsync();
                            System.Console.WriteLine("Журнал успешно обновлен");
                        }
                        else
                        {
                            System.Console.WriteLine("Журнал не найден");
                        }
                        break;

                    case "3":
                        var reader = await context.Readers.FindAsync(id);
                        if (reader != null)
                        {
                            System.Console.Write("Новое имя: ");
                            reader.FirstName = System.Console.ReadLine();
                            System.Console.Write("Новая фамилия: ");
                            reader.LastName = System.Console.ReadLine();
                            await context.SaveChangesAsync();
                            System.Console.WriteLine("Читатель успешно обновлен");
                        }
                        else
                        {
                            System.Console.WriteLine("Читатель не найден");
                        }
                        break;

                    case "4":
                        var record = await context.BorrowingRecords.FindAsync(id);
                        if (record != null)
                        {
                            System.Console.Write("Вернуть книгу? (да/нет): ");
                            record.IsReturned = System.Console.ReadLine().ToLower() == "да";
                            if (record.IsReturned)
                            {
                                record.ReturnDate = DateTime.UtcNow;
                            }
                            await context.SaveChangesAsync();
                            System.Console.WriteLine("Запись выдачи успешно обновлена");
                        }
                        else
                        {
                            System.Console.WriteLine("Запись выдачи не найдена");
                        }
                        break;

                    default:
                        System.Console.WriteLine("Неверный выбор");
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при обновлении: {ex.Message}");
            }
        }

        static async Task DeleteRecord(LibraryContext context)
        {
            System.Console.WriteLine("\nВыберите тип записи для удаления:");
            System.Console.WriteLine("1. Книга");
            System.Console.WriteLine("2. Журнал");
            System.Console.WriteLine("3. Читатель");
            System.Console.WriteLine("4. Запись выдачи");

            var choice = System.Console.ReadLine();
            System.Console.Write("Введите ID записи для удаления: ");
            if (!Guid.TryParse(System.Console.ReadLine(), out Guid id))
            {
                System.Console.WriteLine("Неверный формат ID");
                return;
            }

            try
            {
                switch (choice)
                {
                    case "1":
                        var book = await context.Books.FindAsync(id);
                        if (book != null)
                        {
                            context.Books.Remove(book);
                            await context.SaveChangesAsync();
                            System.Console.WriteLine("Книга успешно удалена");
                        }
                        else
                        {
                            System.Console.WriteLine("Книга не найдена");
                        }
                        break;

                    case "2":
                        var magazine = await context.Magazines.FindAsync(id);
                        if (magazine != null)
                        {
                            context.Magazines.Remove(magazine);
                            await context.SaveChangesAsync();
                            System.Console.WriteLine("Журнал успешно удален");
                        }
                        else
                        {
                            System.Console.WriteLine("Журнал не найден");
                        }
                        break;

                    case "3":
                        var reader = await context.Readers.FindAsync(id);
                        if (reader != null)
                        {
                            context.Readers.Remove(reader);
                            await context.SaveChangesAsync();
                            System.Console.WriteLine("Читатель успешно удален");
                        }
                        else
                        {
                            System.Console.WriteLine("Читатель не найден");
                        }
                        break;

                    case "4":
                        var record = await context.BorrowingRecords.FindAsync(id);
                        if (record != null)
                        {
                            context.BorrowingRecords.Remove(record);
                            await context.SaveChangesAsync();
                            System.Console.WriteLine("Запись выдачи успешно удалена");
                        }
                        else
                        {
                            System.Console.WriteLine("Запись выдачи не найдена");
                        }
                        break;

                    default:
                        System.Console.WriteLine("Неверный выбор");
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при удалении: {ex.Message}");
            }
        }
    }
}
