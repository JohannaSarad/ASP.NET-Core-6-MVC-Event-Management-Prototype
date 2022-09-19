using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.Models;
using Microsoft.AspNetCore.Mvc;
using JSarad_C868_Capstone.ViewModels;
namespace JSarad_C868_Capstone.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _db;

        public EmployeeController(AppDbContext db)
        {
            _db = db;
            //_db can be used to access database tables (Entity Framework)
        }

        //Get /Employee
        public IActionResult Index()
        {
            IEnumerable<Employee> employeeList = _db.Employees;
            return View(employeeList);
        }

        //Get /Employee/Add
        public IActionResult Add()
        {
            EmployeeViewModel viewModel = new EmployeeViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(EmployeeViewModel viewModel)
        {
            //added viewModel and daysToChar conversion for employee.Availability (May or may not work here)
            //EmployeeViewModel viewModel = new EmployeeViewModel();
   
            string daysToChars = "";
            if (viewModel.Monday)
            {
                daysToChars += "M";
            }
            if (viewModel.Tuesday)
            {
                daysToChars += "T";
            }
            if (viewModel.Wednesday)
            {
                daysToChars += "W";
            }
            if (viewModel.Thursday)
            {
                daysToChars += "R";
            }
            if (viewModel.Friday)
            {
                daysToChars += "F";
            }
            if (viewModel.Saturday)
            {
                daysToChars += "S";
            }
            if (viewModel.Sunday)
            {
                daysToChars += "U";
            }

            viewModel.Employee.Availability = daysToChars;
            
            //further validation explained in ClientController
            //validation
            if (ModelState.IsValid)
            {
                //adding an employee to database
                _db.Employees.Add(viewModel.Employee);

                _db.SaveChanges();
                //redirecting to the main employee page with list of employees
                return RedirectToAction("Index");
            }
            else
            {
                return View(viewModel);
            }
        }

        //Get /Employee/Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var selectedEmployee = _db.Employees.Find(id);

            //check if selectedEmployee is null
            if (selectedEmployee == null)
            {
                return NotFound();
            }
            return View(selectedEmployee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee)
        {
            //validation
            if (ModelState.IsValid)
            {
                //updatng a employee in the database
                _db.Employees.Update(employee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(employee);
            }
        }

        //Get /Employee/Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var selectedEmployee = _db.Employees.Find(id);

            if (selectedEmployee == null)
            {
                return NotFound();
            }
            return View(selectedEmployee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var selectedEmployee = _db.Employees.Find(id);
            if (selectedEmployee == null)
            {
                return NotFound();
            }
            _db.Employees.Remove(selectedEmployee);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

