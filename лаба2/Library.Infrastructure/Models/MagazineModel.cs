using System;

namespace Library.Infrastructure.Models
{
    public class MagazineModel : LibraryItemModel
    {
        public string ISSN { get; set; }
        public int IssueNumber { get; set; }
        public string Category { get; set; }
        public string Editor { get; set; }
    }
} 