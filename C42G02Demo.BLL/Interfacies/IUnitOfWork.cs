using System.Threading.Tasks;

namespace C42G02Demo.BLL.Interfacies
{
    public interface IUnitOfWork
    {
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        public Task<int> CompleteAsync();
    }
}
