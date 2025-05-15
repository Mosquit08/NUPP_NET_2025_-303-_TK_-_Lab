using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Common
{
    public interface ICrudServiceAsync<T>
    {
        Task CreateAsync(T element);
        Task<T> ReadAsync(Guid id);
        Task<IEnumerable<T>> ReadAllAsync();
        Task UpdateAsync(T element);
        Task RemoveAsync(T element);
        Task SaveAsync(string filePath);
        Task LoadAsync(string filePath);
        Task<IEnumerable<T>> GetPageAsync(int pageNumber, int pageSize);
    }
} 