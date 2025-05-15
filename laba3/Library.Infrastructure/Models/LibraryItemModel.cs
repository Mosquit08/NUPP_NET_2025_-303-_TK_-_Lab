using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Infrastructure.Models
{
    public abstract class LibraryItemModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Навигационные свойства
        public virtual ICollection<BorrowingRecordModel> BorrowingRecords { get; set; }

        protected LibraryItemModel()
        {
            Id = Guid.NewGuid();
            Title = string.Empty;
            Publisher = string.Empty;
            CreatedAt = DateTime.UtcNow;
            BorrowingRecords = new List<BorrowingRecordModel>();
        }
    }
} 