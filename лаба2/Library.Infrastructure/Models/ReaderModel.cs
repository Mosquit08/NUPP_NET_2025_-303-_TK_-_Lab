using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Infrastructure.Models
{
    public class ReaderModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Навигационные свойства
        public virtual ICollection<BorrowingRecordModel> BorrowingRecords { get; set; }

        public ReaderModel()
        {
            Id = Guid.NewGuid();
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            RegistrationDate = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            BorrowingRecords = new List<BorrowingRecordModel>();
        }
    }
} 