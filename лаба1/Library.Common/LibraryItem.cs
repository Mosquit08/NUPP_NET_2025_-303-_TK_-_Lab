using System;

namespace Library.Common
{
    public abstract class LibraryItem
    {
        // Статическое поле для отслеживания общего количества предметов
        public static int TotalItems { get; private set; }

        // Статический конструктор
        static LibraryItem()
        {
            TotalItems = 0;
        }

        // Событие для уведомления о статусе предмета
        public event EventHandler<string> StatusChanged;

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; private set; }

        // Конструктор
        protected LibraryItem()
        {
            Id = Guid.NewGuid();
            IsAvailable = true;
            TotalItems++;
        }

        // Метод для изменения статуса доступности
        public virtual void ChangeAvailability(bool isAvailable)
        {
            IsAvailable = isAvailable;
            StatusChanged?.Invoke(this, $"Статус предмета {Title} изменен на {(isAvailable ? "доступен" : "недоступен")}");
        }

        // Абстрактный метод для получения типа предмета
        public abstract string GetItemType();
    }
} 