using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Infrastructure.Models
{
    public class BorrowingRecordModel
    {
        public Guid Id { get; set; }
        public Guid ReaderId { get; set; }
        public Guid ItemId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Навигационные свойства
        public virtual ReaderModel Reader { get; set; }
        public virtual LibraryItemModel Item { get; set; }

        public BorrowingRecordModel()
        {
            Id = Guid.NewGuid();
            BorrowDate = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            IsReturned = false;
        }
    }
} 