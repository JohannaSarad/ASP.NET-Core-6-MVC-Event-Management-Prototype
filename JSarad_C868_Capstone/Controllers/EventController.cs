using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.Models;
using JSarad_C868_Capstone.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JSarad_C868_Capstone.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _db;

        public EventController(AppDbContext db)
        {
            _db = db;
        }

        //Get /Event
        public IActionResult Index()
        {
            //EventListViewModel viewModel = new EventListViewModel();

            //IEnumerable<Event> eventList = _db.Events;
            //IEnumerable<Client> clientList = _db.Clients;
            var result = from e in _db.Events
                         join c in _db.Clients on e.ClientId equals c.Id
                         select new EventListViewModel
                         {
                             Id = e.Id,
                             StartDate = e.EventStart,
                             EndDate = e.EventEnd,
                             Type = e.Type,
                             ContactName = c.Name,
                             Phone = c.Phone
                         }; //.ToList();
            return View(result);
        }

        //Get /Event/Add
        public IActionResult Add()
        {
            EventViewModel viewModel = new EventViewModel();
            viewModel.EmployeeList = _db.Employees; 
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(EventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //adding an event to database
                _db.Events.Add(viewModel.Event);

                _db.SaveChanges();
                //redirecting to the main event page with list of events
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
            var selectedEvent = _db.Events.Find(id);

            //check if selectedEmployee is null
            if (selectedEvent == null)
            {
                return NotFound();
            }

            EventViewModel viewModel = new EventViewModel();
            viewModel.Event = selectedEvent;
            
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EventViewModel viewModel)
        {
            //viewModel.Employee.Availability = daysToChars;
            //validation
            if (ModelState.IsValid)
            {
                //updatng a employee in the database
                _db.Events.Update(viewModel.Event);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(viewModel);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var selectedEvent = _db.Events.Find(id);
            if (selectedEvent == null)
            {
                return NotFound();
            }
            _db.Events.Remove(selectedEvent);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            var clients =  (from client in _db.Clients
                           where client.Name.StartsWith(prefix)
                           select new
                           {
                               label = client.Name,
                               val = client.Name
                           }).Take(5).ToList();

            return Json(clients);
        }
    }
}
