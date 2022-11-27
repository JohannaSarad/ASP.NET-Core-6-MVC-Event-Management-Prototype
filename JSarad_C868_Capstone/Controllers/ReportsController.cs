using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JSarad_C868_Capstone.Models;

namespace JSarad_C868_Capstone.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly AppDbContext _db;

        public ReportsController(AppDbContext db)
        {
            _db = db;
        }

        
        public IActionResult Index()
        {
            
            ReportsViewModel viewModel = new ReportsViewModel();
            viewModel.UserSelectList = BindSelectList();
            viewModel.StartDate = DateTime.Now;
            viewModel.EndDate = DateTime.Now;
            viewModel.EventList = _db.Events.OrderBy(e => e.StartTime).ToList();
            
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(ReportsViewModel viewModel)
        {
            ModelState.Clear();
            viewModel.UserSelectList = BindSelectList();
            int userId = Convert.ToInt32(viewModel.SelectedUser);
           
            //determine which filter result to return based on user input
            if (viewModel.OrderByPlanner)
            {
                if (userId > 0)
                {
                    if (viewModel.OrderByDates == true)
                    {
                        viewModel.EventList = (from e in _db.Events
                                               where e.CreatedBy == userId
                                               && e.StartTime.Date >= viewModel.StartDate.Date
                                               && e.EndTime.Date <= viewModel.EndDate.Date
                                               orderby e.StartTime
                                               select e).ToList();
                        return View(viewModel);
                    }
                    viewModel.EventList = _db.Events.Where(e => e.CreatedBy == userId).OrderBy(e => e.StartTime).ToList();
                    return View(viewModel);
                }
                viewModel.EventList = _db.Events.OrderBy(e => e.StartTime).ToList();
                TempData["Error"] = "Please select a planner";
                return View(viewModel);
            }

            if (viewModel.OrderByDates)
            {
                viewModel.EventList = (from e in _db.Events
                                       where e.StartTime.Date >= viewModel.StartDate.Date
                                       && e.EndTime.Date <= viewModel.EndDate.Date
                                       orderby e.StartTime
                                       select e).ToList();
                return View(viewModel);
            }
            viewModel.EventList = _db.Events.OrderBy(e => e.StartTime).ToList();
            TempData["Error"] = "Please select how you would like to filter your results from the provided checkboxes";
            return View(viewModel);
        }

        public List<SelectListItem> BindSelectList()
        {
            var usersData = _db.Users;
            List<SelectListItem> UserSelectList = new List<SelectListItem>();
            UserSelectList.Insert(0, new SelectListItem
            {
                Text = "--select planner--",
                Value = "0"
            });

            foreach (User user in usersData)
            {
                UserSelectList.Add(new SelectListItem
                {
                    Text = user.Username,
                    Value = user.Id.ToString()
                });
            }
            return UserSelectList;
        }
    }
}
