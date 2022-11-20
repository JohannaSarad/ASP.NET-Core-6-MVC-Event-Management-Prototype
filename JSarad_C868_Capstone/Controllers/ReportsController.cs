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
            //var usersData = _db.Users;
            ReportsViewModel viewModel = new ReportsViewModel();
            //viewModel.UserSelectList = new List<SelectListItem>();
            viewModel.UserSelectList = BindSelectList();
            viewModel.StartDate = DateTime.Now;
            viewModel.EndDate = DateTime.Now;
            viewModel.EventList = _db.Events.ToList();
            //viewModel.UserSelectList.Insert(0, new SelectListItem
            //{
            //    Text = "All Planners",
            //    Value = "0"
            //});

            //foreach (User user in usersData)
            //{
            //    viewModel.UserSelectList.Add(new SelectListItem
            //    {
            //        Text = user.Username,
            //        Value = user.Id.ToString()
            //    });
            //}
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Search(ReportsViewModel viewModel)
        {
            ModelState.Clear();
            viewModel.UserSelectList = BindSelectList();
            int userId = Convert.ToInt32(viewModel.SelectedUser);
            if (userId == 0)
            {
                viewModel.EventList = (from e in _db.Events
                                       where e.StartTime >= viewModel.StartDate
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
                                           CreatedBy = e.CreatedBy,
                                           CreatedOn = e.CreatedOn,
                                           ClientId = e.ClientId
                                       }).ToList();

            }
            else
            {
                //viewModel.EventList (temporarilyt switched with queryResult
                viewModel.EventList = (from e in _db.Events
                                       where
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
            }
            return View("Index", viewModel);
            //if (!string.IsNullOrEmpty(search))
            //{
            //    searchQuery = searchQuery.Where(e => e.Type.Contains(search)).OrderBy(s => s.StartTime);
            //}
            //return View(await queryFilter.AsNoTracking().ToListAsync());
        }

        public List<SelectListItem> BindSelectList()
        {
            var usersData = _db.Users;
            List<SelectListItem> UserSelectList = new List<SelectListItem>();
            UserSelectList.Insert(0, new SelectListItem
            {
                Text = "All Planners",
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
