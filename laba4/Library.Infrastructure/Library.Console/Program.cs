using System;
using System.Threading.Tasks;
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
            try
            {
                System.Console.WriteLine("Начало работы программы...");
                
                // Создаем настройки базы данных
                var dbSettings = new DatabaseSettings
                {
                    Server = "localhost",
                    Database = "library",
                    User = "root",
                    Password = "",
                    Port = 3306
                };
                
                System.Console.WriteLine("Настройки базы данных созданы");
                System.Console.WriteLine($"Строка подключения: {dbSettings.GetConnectionString()}");

                var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
                var connectionString = $"Server={dbSettings.Server};Port={dbSettings.Port};Database={dbSettings.Database};User={dbSettings.User};Password={dbSettings.Password};";
                
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                
                using var context = new LibraryContext(optionsBuilder.Options);
                System.Console.WriteLine("Контекст базы данных создан");

                // Создаем тестовую книгу
                System.Console.WriteLine("Начинаем создание тестовой книги...");
                var book = new BookModel
                {
                    Title = "Test Book",
                    Author = "Test Author",
                    Publisher = "Test Publisher",
                    Year = 2024,
                    ISBN = "1234567890123",
                    PageCount = 100,
                    Genre = "Test Genre",
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow
                };
                System.Console.WriteLine("Тестовая книга создана в памяти");

                // Добавляем книгу в базу данных
                System.Console.WriteLine("Добавление сущности типа BookModel...");
                context.Books.Add(book);
                System.Console.WriteLine("Сущность добавлена в DbSet");
                await context.SaveChangesAsync();
                System.Console.WriteLine("Изменения сохранены успешно");

                // Получаем все книги
                System.Console.WriteLine("Получаем список всех книг...");
                var books = await context.Books.ToListAsync();
                System.Console.WriteLine("\nСписок всех книг:");
                foreach (var b in books)
                {
                    System.Console.WriteLine($"- {b.Title} by {b.Author}");
                }

                // Получаем всех читателей
                System.Console.WriteLine("\nСписок всех читателей:");
                var readers = await context.Readers.ToListAsync();
                foreach (var reader in readers)
                {
                    System.Console.WriteLine($"- {reader.FirstName} {reader.LastName}");
                }

                // Получаем все записи о выдаче
                System.Console.WriteLine("\nСписок всех записей выдачи:");
                var records = await context.BorrowingRecords.ToListAsync();
                foreach (var record in records)
                {
                    System.Console.WriteLine($"- {record.BorrowDate} - {record.ReturnDate}");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Произошла ошибка: {ex.Message}");
                System.Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            System.Console.WriteLine("\nНажмите любую клавишу для выхода...");
            System.Console.ReadKey();
        }
    }
}
