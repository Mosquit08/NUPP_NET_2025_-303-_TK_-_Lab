using Microsoft.EntityFrameworkCore;
using Library.REST.Models;

namespace Library.REST.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<BookModel> Books { get; set; }
        public DbSet<MagazineModel> Magazines { get; set; }
        public DbSet<ReaderModel> Readers { get; set; }
        public DbSet<BorrowingRecordModel> BorrowingRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связей для BorrowingRecordModel
            modelBuilder.Entity<BorrowingRecordModel>()
                .HasOne(br => br.Book)
                .WithMany()
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowingRecordModel>()
                .HasOne(br => br.Magazine)
                .WithMany()
                .HasForeignKey(br => br.MagazineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowingRecordModel>()
                .HasOne(br => br.Reader)
                .WithMany()
                .HasForeignKey(br => br.ReaderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 