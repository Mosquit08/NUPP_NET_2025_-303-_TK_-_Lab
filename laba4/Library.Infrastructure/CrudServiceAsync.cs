using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Infrastructure.Models;

namespace Library.Infrastructure
{
    public interface ICrudServiceAsync<T>
    {
        Task<bool> CreateAsync(T element);
        Task<T> ReadAsync(Guid id);
        Task<IEnumerable<T>> ReadAllAsync();
        Task<IEnumerable<T>> ReadAllAsync(int page, int amount);
        Task<bool> UpdateAsync(T element);
        Task<bool> RemoveAsync(T element);
        Task<bool> SaveAsync();
    }

    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public CrudServiceAsync(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateAsync(T element)
        {
            try
            {
                await _repository.AddAsync(element);
                await SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<T> ReadAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var all = await _repository.GetAllAsync();
            return all.Skip((page - 1) * amount).Take(amount);
        }

        public async Task<bool> UpdateAsync(T element)
        {
            try
            {
                await _repository.UpdateAsync(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveAsync(T element)
        {
            try
            {
                await _repository.DeleteAsync(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                // В данном случае сохранение происходит автоматически в методах репозитория
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 