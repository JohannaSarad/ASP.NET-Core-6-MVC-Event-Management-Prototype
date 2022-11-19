using JSarad_C868_Capstone.Models;

namespace JSarad_C868_Capstone.Data.Interfaces
{
    public interface IEmployeeRepository : IDisposable
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        void Add(Employee employee);
        Employee Update(int id, Employee employee);
        void Delete(int id);
    }
}
