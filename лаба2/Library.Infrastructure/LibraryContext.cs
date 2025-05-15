using Microsoft.EntityFrameworkCore;
using Library.Infrastructure.Models;
using System;

namespace Library.Infrastructure
{
    public class LibraryContext : DbContext
    {
        public DbSet<BookModel> Books { get; set; }
        public DbSet<MagazineModel> Magazines { get; set; }
        public DbSet<ReaderModel> Readers { get; set; }
        public DbSet<BorrowingRecordModel> BorrowingRecords { get; set; }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
            System.Console.WriteLine("Инициализация LibraryContext...");
            System.Console.WriteLine("LibraryContext инициализирован");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table-per-Type (TPT) стратегия
            modelBuilder.Entity<LibraryItemModel>().ToTable("LibraryItems");
            modelBuilder.Entity<BookModel>().ToTable("Books");
            modelBuilder.Entity<MagazineModel>().ToTable("Magazines");

            // Конфигурация для LibraryItemModel
            modelBuilder.Entity<LibraryItemModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Publisher).IsRequired();
                entity.Property(e => e.Year).IsRequired();
                entity.Property(e => e.IsAvailable).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // Конфигурация для BookModel
            modelBuilder.Entity<BookModel>(entity =>
            {
                entity.Property(e => e.Author).IsRequired();
                entity.Property(e => e.ISBN).IsRequired();
                entity.Property(e => e.Genre).IsRequired();
                entity.Property(e => e.PageCount).IsRequired();
            });

            // Конфигурация для MagazineModel
            modelBuilder.Entity<MagazineModel>(entity =>
            {
                entity.Property(e => e.ISSN).IsRequired();
                entity.Property(e => e.Category).IsRequired();
                entity.Property(e => e.Editor).IsRequired();
            });

            // Конфигурация для ReaderModel
            modelBuilder.Entity<ReaderModel>(entity =>
            {
                entity.ToTable("Readers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.PhoneNumber).IsRequired();
                entity.Property(e => e.RegistrationDate).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // Конфигурация для BorrowingRecordModel
            modelBuilder.Entity<BorrowingRecordModel>(entity =>
            {
                entity.ToTable("BorrowingRecords");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ReaderId).IsRequired();
                entity.Property(e => e.ItemId).IsRequired();
                entity.Property(e => e.BorrowDate).IsRequired();
                entity.Property(e => e.IsReturned).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(e => e.Item)
                    .WithMany(i => i.BorrowingRecords)
                    .HasForeignKey(e => e.ItemId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Reader)
                    .WithMany(r => r.BorrowingRecords)
                    .HasForeignKey(e => e.ReaderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
} 