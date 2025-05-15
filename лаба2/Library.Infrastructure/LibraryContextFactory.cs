using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Library.Infrastructure
{
    public class LibraryContextFactory : IDesignTimeDbContextFactory<LibraryContext>
    {
        public LibraryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            var connectionString = "Server=localhost;Port=3306;Database=library;User=root;Password=;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            return new LibraryContext(optionsBuilder.Options);
        }
    }
} 