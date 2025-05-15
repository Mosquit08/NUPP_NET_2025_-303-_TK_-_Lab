using System;

namespace Library.Infrastructure.Models
{
    public class BookModel : LibraryItemModel
    {
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int PageCount { get; set; }
        public string Genre { get; set; }

        public BookModel()
        {
            Author = string.Empty;
            ISBN = string.Empty;
            Genre = string.Empty;
        }
    }
} 