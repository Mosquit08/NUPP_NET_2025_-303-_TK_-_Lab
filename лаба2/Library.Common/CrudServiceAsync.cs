using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Library.Common
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly List<T> _items;
        private readonly string _typeName;
        private static readonly object _lock = new object();

        public CrudServiceAsync()
        {
            _items = new List<T>();
            _typeName = typeof(T).Name;
        }

        public Task CreateAsync(T element)
        {
            _items.Add(element);
            return Task.CompletedTask;
        }

        public Task<T> ReadAsync(Guid id)
        {
            var property = typeof(T).GetProperty("Id");
            if (property == null)
                throw new InvalidOperationException($"Тип {_typeName} не содержит свойство Id");

            return Task.FromResult(_items.FirstOrDefault(item => 
                (Guid)property.GetValue(item) == id));
        }

        public Task<IEnumerable<T>> ReadAllAsync()
        {
            return Task.FromResult(_items.AsEnumerable());
        }

        public Task UpdateAsync(T element)
        {
            var property = typeof(T).GetProperty("Id");
            if (property == null)
                throw new InvalidOperationException($"Тип {_typeName} не содержит свойство Id");

            var id = (Guid)property.GetValue(element);
            var index = _items.FindIndex(item => 
                (Guid)property.GetValue(item) == id);

            if (index != -1)
            {
                _items[index] = element;
            }

            return Task.CompletedTask;
        }

        public Task RemoveAsync(T element)
        {
            _items.Remove(element);
            return Task.CompletedTask;
        }

        public async Task SaveAsync(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            lock (_lock)
            {
                var json = JsonSerializer.Serialize(_items, options);
                File.WriteAllText(filePath, json);
            }

            await Task.CompletedTask;
        }

        public async Task LoadAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json;
                lock (_lock)
                {
                    json = File.ReadAllText(filePath);
                }

                var items = JsonSerializer.Deserialize<List<T>>(json);
                _items.Clear();
                _items.AddRange(items);
            }

            await Task.CompletedTask;
        }

        public Task<IEnumerable<T>> GetPageAsync(int pageNumber, int pageSize)
        {
            return Task.FromResult(_items
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize));
        }
    }
} 