using System;

namespace Library.REST.Models
{
    public class BorrowingRecordModel : BaseModel
    {
        public Guid BookId { get; set; }
        public Guid MagazineId { get; set; }
        public Guid ReaderId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }

        // Навигационные свойства
        public BookModel Book { get; set; }
        public MagazineModel Magazine { get; set; }
        public ReaderModel Reader { get; set; }
    }
} 