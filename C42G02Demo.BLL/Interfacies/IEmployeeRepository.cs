using C42G02Demo.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C42G02Demo.BLL.Interfacies
{
    public interface IEmployeeRepository :IGenericRepository<Employee>
    {
        //IQueryable -> Can't be asyncronous cuz it's doin it's filteration in the database
        IQueryable<Employee> GetEmployeesByAddress(string address);
        IQueryable<Employee> GetEmployeesByName(string SearchValue);
    }
}
