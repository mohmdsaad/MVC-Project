using C42G02Demo.BLL.Interfacies;
using C42G02Demo.DAL.Data;
using C42G02Demo.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace C42G02Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository
    {
        private readonly AppDbContext _dbContext;

        public EmployeeRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
        => _dbContext.Employees.Where(e => e.Address == address);

        public IQueryable<Employee> GetEmployeesByName(string SearchValue)
        => _dbContext.Employees.Where(e=>e.Name.ToLower().Contains(SearchValue.ToLower()));
    }
}