using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Library.Infrastructure.Models;

namespace Library.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly LibraryContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(LibraryContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                System.Console.WriteLine($"Добавление сущности типа {typeof(T).Name}...");
                await _dbSet.AddAsync(entity);
                System.Console.WriteLine("Сущность добавлена в DbSet");
                await _context.SaveChangesAsync();
                System.Console.WriteLine("Изменения сохранены в базе данных");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при добавлении сущности: {ex.Message}");
                System.Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
} 