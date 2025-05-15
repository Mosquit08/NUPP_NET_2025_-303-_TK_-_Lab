using System;

namespace Library.Common
{
    public class BorrowingRecord
    {
        public Guid Id { get; set; }
        public Guid ReaderId { get; set; }
        public Guid ItemId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }

        // Конструктор
        public BorrowingRecord(Guid readerId, Guid itemId)
        {
            Id = Guid.NewGuid();
            ReaderId = readerId;
            ItemId = itemId;
            BorrowDate = DateTime.Now;
            IsReturned = false;
        }

        // Метод для возврата предмета
        public void Return()
        {
            if (!IsReturned)
            {
                ReturnDate = DateTime.Now;
                IsReturned = true;
            }
        }

        // Метод для получения информации о записи
        public string GetRecordInfo()
        {
            return $"Запись выдачи: ID={Id}, Читатель={ReaderId}, Предмет={ItemId}, " +
                   $"Дата выдачи={BorrowDate}, Возвращен={IsReturned}, " +
                   $"Дата возврата={(ReturnDate.HasValue ? ReturnDate.Value.ToString() : "Не возвращен")}";
        }
    }
} 