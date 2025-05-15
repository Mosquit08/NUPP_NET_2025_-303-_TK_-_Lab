using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Library.Common
{
    public class CrudService<T> : ICrudService<T> where T : class
    {
        private readonly List<T> _items;
        private readonly string _typeName;

        public CrudService()
        {
            _items = new List<T>();
            _typeName = typeof(T).Name;
        }

        public void Create(T element)
        {
            _items.Add(element);
        }

        public T Read(Guid id)
        {
            var property = typeof(T).GetProperty("Id");
            if (property == null)
                throw new InvalidOperationException($"Тип {_typeName} не содержит свойство Id");

            return _items.FirstOrDefault(item => 
                (Guid)property.GetValue(item) == id);
        }

        public IEnumerable<T> ReadAll()
        {
            return _items;
        }

        public void Update(T element)
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
        }

        public void Remove(T element)
        {
            _items.Remove(element);
        }

        public void Save(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(_items, options);
            File.WriteAllText(filePath, json);
        }

        public void Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var items = JsonSerializer.Deserialize<List<T>>(json);
                _items.Clear();
                _items.AddRange(items);
            }
        }
    }
} 