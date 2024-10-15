using C42G02Demo.BLL.Interfacies;
using C42G02Demo.DAL.Data;
using C42G02Demo.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C42G02Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task AddAsync(T item)
            => await _dbContext.AddAsync(item);

        public void Delete(T item)
            => _dbContext.Remove(item);

        public async Task<IEnumerable<T>> GetAllAsync()
        { 
            if(typeof(T) == typeof(Employee))
                return (IEnumerable<T>) await _dbContext.Employees.Include(e=>e.Department).ToListAsync();

            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
            => await _dbContext.Set<T>().FindAsync(id);

        public void Update(T item)
            => _dbContext.Update(item);
    }
}
