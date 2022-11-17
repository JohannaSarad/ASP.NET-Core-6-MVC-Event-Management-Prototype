using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.Models;
using Microsoft.AspNetCore.Mvc;
using JSarad_C868_Capstone.ViewModels;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace JSarad_C868_Capstone.Controllers
{
    //FIX ME!!! deselect object after edit
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _db;

        public EmployeeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var employees = _db.Employees;
            return View(employees);
        }

        //search function 
        //Get: /Employee/Index/{string}
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            ViewData["GetEmployee"] = search;
            var searchQuery = from e in _db.Employees select e;
            if (!string.IsNullOrEmpty(search))
            {
                searchQuery = searchQuery.Where(e => e.Name.Contains(search) || e.Email.Contains(search));
            }
            return View(await searchQuery.AsNoTracking().ToListAsync());
        }

        //Get: /Employee/Modify/{id}
        [HttpGet]
        public IActionResult Modify(int id)
        {
            EmployeeViewModel viewModel = new EmployeeViewModel();
            if (id == 0)
            {
                viewModel.Employee = new Employee();
                viewModel.Title = "Add Employee";
            }
            else
            {
                viewModel.Employee = _db.Employees.Find(id);
                viewModel = CharsToDays(viewModel);
                viewModel.Title = "Edit Employee";
            }
            return PartialView("_ModifyEmployeeModalPartial", viewModel); ;
        }

        //POST: /Employee/Modify/{model}
        [HttpPost]
        public IActionResult Modify(EmployeeViewModel viewModel)
        {
            viewModel.Employee.Availability = DaysToChars(viewModel);

            if (viewModel.Employee.Phone != null)
            {
                char firstCharacter = viewModel.Employee.Phone[0];
                //check that ph
                if (firstCharacter == '0')
                {
                    ModelState.AddModelError("Employee.Phone", "The Phone field is not a valid phone number");
                }
            }
            

            if (ModelState.IsValid)
            {
                if (viewModel.Employee.Id == 0)
                {
                    _db.Employees.Add(viewModel.Employee);
                    _db.SaveChanges();
                }
                else
                {
                    _db.Employees.Update(viewModel.Employee);
                    _db.SaveChanges();
                }
                return Ok(true);
            }
            return PartialView("_ModifyEmployeeModalPartial", viewModel);
        }

        //POST: Delete
        [HttpPost]
        //[ValidateAntiForgeryToken]
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

        public IActionResult EmployeeSchedule(int? id)
        {
            EmployeeScheduleViewModel viewModel= new EmployeeScheduleViewModel();
            viewModel.Employee = _db.Employees.Find(id);
            viewModel.Schedule = (from s in _db.Schedules
                                  where s.Id == id
                                  select new Schedule
                                  {
                                      Id = s.Id,
                                      EmployeeId = s.EmployeeId,
                                      StartTime = s.StartTime,
                                      EndTime = s.EndTime,
                                  }).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Selection(int id)
        {
            var selectedEmployee = _db.Employees.Find(id);
            //if (selectedEmployee != null)
            //{
                return Json(selectedEmployee.Name);
            //}
            //return View("Index", id);

        }

        public string DaysToChars(EmployeeViewModel viewModel)
        {
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

            return daysToChars;
        }

        public EmployeeViewModel CharsToDays(EmployeeViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.Employee.Availability))
            {
                if (viewModel.Employee.Availability.Contains("M"))
                {
                    viewModel.Monday = true;
                }
                if (viewModel.Employee.Availability.Contains("T"))
                {
                    viewModel.Tuesday = true;
                }
                if (viewModel.Employee.Availability.Contains("W"))
                {
                    viewModel.Wednesday = true;
                }
                if (viewModel.Employee.Availability.Contains("R"))
                {
                    viewModel.Thursday = true;
                }
                if (viewModel.Employee.Availability.Contains("F"))
                {
                    viewModel.Friday = true;
                }
                if (viewModel.Employee.Availability.Contains("S"))
                {
                    viewModel.Saturday = true;
                }
                if (viewModel.Employee.Availability.Contains("U"))
                {
                    viewModel.Sunday = true;
                }
            }
            return viewModel;
        }
        //Get /Employee/Edit
        
        
    }
}
//public IActionResult Edit(int id)
//{
//    if (id == null || id == 0)
//    {
//        return NotFound();
//    }
//    var selectedEmployee = _db.Employees.Find(id);

//    //check if selectedEmployee is null
//    if (selectedEmployee == null)
//    {
//        return NotFound();
//    }

//    EmployeeViewModel viewModel = new EmployeeViewModel();
//    viewModel.Employee = selectedEmployee;

//    if (!string.IsNullOrEmpty(viewModel.Employee.Availability)) {
//        if (viewModel.Employee.Availability.Contains("M"))
//        {
//            viewModel.Monday = true;
//        }
//        if (viewModel.Employee.Availability.Contains("T"))
//        {
//            viewModel.Tuesday = true;
//        }
//        if (viewModel.Employee.Availability.Contains("W"))
//        {
//            viewModel.Wednesday = true;
//        }
//        if (viewModel.Employee.Availability.Contains("R"))
//        {
//            viewModel.Thursday = true;
//        }
//        if (viewModel.Employee.Availability.Contains("F"))
//        {
//            viewModel.Friday = true;
//        }
//        if (viewModel.Employee.Availability.Contains("S"))
//        {
//            viewModel.Saturday = true;
//        }
//        if (viewModel.Employee.Availability.Contains("U"))
//        {
//            viewModel.Sunday = true;
//        }
//    }
//    //return Ok();
//    return View("_EditEmployeeModalPartial", viewModel);
//}

//[HttpPost]
//[ValidateAntiForgeryToken]
//public IActionResult Edit(EmployeeViewModel viewModel)
//{
//    string daysToChars = "";
//    if (viewModel.Monday)
//    {
//        daysToChars += "M";
//    }
//    if (viewModel.Tuesday)
//    {
//        daysToChars += "T";
//    }
//    if (viewModel.Wednesday)
//    {
//        daysToChars += "W";
//    }
//    if (viewModel.Thursday)
//    {
//        daysToChars += "R";
//    }
//    if (viewModel.Friday)
//    {
//        daysToChars += "F";
//    }
//    if (viewModel.Saturday)
//    {
//        daysToChars += "S";
//    }
//    if (viewModel.Sunday)
//    {
//        daysToChars += "U";
//    }

//    viewModel.Employee.Availability = daysToChars;
//    //validation
//    if (ModelState.IsValid)
//    {
//        //updatng a employee in the database
//        _db.Employees.Update(viewModel.Employee);
//        _db.SaveChanges();
//        return RedirectToAction("Index");
//    }
//    else
//    {
//        return View(viewModel);
//    }
//}

////Get /Employee/Add
//[HttpGet]
//public IActionResult Add()
//{
//    EmployeeViewModel viewModel = new EmployeeViewModel();
//    viewModel.Employee = new Employee();
//    return PartialView("_AddEmployeeModalPartial", viewModel); ;
//}

//[HttpPost]
////[ValidateAntiForgeryToken]
//public IActionResult Add(EmployeeViewModel viewModel)
//{
//    viewModel.Employee.Availability = DaysToChars(viewModel);

//    if (ModelState.IsValid)
//    {
//        //adding an employee to database
//        _db.Employees.Add(viewModel.Employee);
//        _db.SaveChanges();
//        return Ok(true);
//    }
//    return PartialView("_AddEmployeeModalPartial", viewModel);
//}