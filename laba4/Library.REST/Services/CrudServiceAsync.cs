using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Library.REST.Models;
using Library.REST.Data;

namespace Library.REST.Services
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : BaseModel
    {
        protected readonly LibraryContext _context;
        protected readonly DbSet<T> _dbSet;

        public CrudServiceAsync(LibraryContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<bool> CreateAsync(T element)
        {
            try
            {
                await _dbSet.AddAsync(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual async Task<T> ReadAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            return await _dbSet.Skip((page - 1) * amount).Take(amount).ToListAsync();
        }

        public virtual async Task<bool> UpdateAsync(T element)
        {
            try
            {
                _dbSet.Update(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual async Task<bool> RemoveAsync(T element)
        {
            try
            {
                _dbSet.Remove(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual async Task<bool> SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 