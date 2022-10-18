using JSarad_C868_Capstone.Models;

namespace JSarad_C868_Capstone.Data
{
    public class DataHelper
    {
        private readonly AppDbContext _db;
        public DataHelper(AppDbContext db)
        {
            _db = db;
        }


        public List<Employee> GetEmployees()
        {
            var employeeList = _db.Employees.ToList();
            return employeeList;
        }
    }
}
