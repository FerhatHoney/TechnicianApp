using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicianApp.Core.Interfaces
{
    // Buraya 'new()' kısıtlamasını eklememiz şart!
    public interface IBaseRepository<T> where T : class, new()
    {
        Task<List<T>> GetAllAsync(string collectionPath);
        Task<T?> GetByIdAsync(string collectionPath, string id);
        Task AddAsync(string collectionPath, T entity, string? id = null);
        Task UpdateAsync(string collectionPath, T entity, string id);
        Task DeleteAsync(string collectionPath, string id);
    }
}