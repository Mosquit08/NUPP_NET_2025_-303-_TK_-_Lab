using System;
using System.Collections.Generic;

namespace Library.Common
{
    public class Reader
    {
        // Статическое поле для отслеживания общего количества читателей
        public static int TotalReaders { get; private set; }

        // Статический конструктор
        static Reader()
        {
            TotalReaders = 0;
        }

        // Делегат для обработки событий читателя
        public delegate void ReaderEventHandler(object sender, string message);

        // События
        public event ReaderEventHandler BookBorrowed;
        public event ReaderEventHandler BookReturned;

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<Guid> BorrowedItems { get; private set; }

        // Конструктор
        public Reader(string firstName, string lastName, string email, string phoneNumber)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            RegistrationDate = DateTime.Now;
            BorrowedItems = new List<Guid>();
            TotalReaders++;
        }

        // Метод для добавления взятого предмета
        public void AddBorrowedItem(Guid itemId)
        {
            BorrowedItems.Add(itemId);
            BookBorrowed?.Invoke(this, $"Читатель {FirstName} {LastName} взял предмет с ID: {itemId}");
        }

        // Метод для возврата предмета
        public void ReturnItem(Guid itemId)
        {
            if (BorrowedItems.Remove(itemId))
            {
                BookReturned?.Invoke(this, $"Читатель {FirstName} {LastName} вернул предмет с ID: {itemId}");
            }
        }

        // Метод для получения полного имени читателя
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        // Метод для получения информации о читателе
        public string GetReaderInfo()
        {
            return $"Читатель: {GetFullName()}, Email: {Email}, Телефон: {PhoneNumber}";
        }
    }
} 