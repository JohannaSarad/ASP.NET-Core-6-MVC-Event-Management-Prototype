using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JSarad_C868_Capstone.Models;
using Microsoft.DiaSymReader;

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
            var usersData = _db.Users;
            ReportsViewModel viewModel = new ReportsViewModel();
            viewModel.UserSelectList = new List<SelectListItem>();
            viewModel.StartDate = DateTime.Now;
            viewModel.EndDate = DateTime.Now;
            viewModel.EventList = _db.Events.ToList();
            viewModel.UserSelectList.Insert(0, new SelectListItem
            {
                Text = "All Planners",
                Value = "0"
            });

            foreach (User user in usersData)
            {
                viewModel.UserSelectList.Add(new SelectListItem
                {
                    Text = user.Username,
                    Value = user.Id.ToString()
                });
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ApplyFilters(ReportsViewModel viewModel)
        {
            ModelState.Clear();
            int userId = Convert.ToInt32(viewModel.SelectedUser);

            viewModel.EventList = (from e in _db.Events where
                                  e.CreatedBy == userId && e.StartTime >= viewModel.StartDate
                                  && e.EndTime <= viewModel.EndDate
                                   select new Event
                                   {
                                       Id = e.Id,
                                       EventName = e.EventName,
                                       EventDate = e.EventDate,
                                       StartTime = e.StartTime,
                                       EndTime = e.EndTime,
                                       Type = e.Type,
                                       Guests = e.Guests,
                                       //Food = e.Food,
                                       //Bar = e.Bar,
                                       CreatedBy = userId,
                                       CreatedOn = e.CreatedOn,    
                                       ClientId = e.ClientId
                                   }).ToList();
            return View(viewModel);
        }
    }
}
