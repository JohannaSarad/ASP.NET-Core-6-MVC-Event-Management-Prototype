using Microsoft.AspNetCore.Mvc;
using JSarad_C868_Capstone.Models;
using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;


namespace JSarad_C868_Capstone.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        //public DataService dataService;
        //public Client SelectedClient { get; set; }
        private readonly AppDbContext _db;

        public ClientController(AppDbContext db)
        {
            _db = db;
           
        }
        
        //retrieves clients from database and populates client table
        public IActionResult Index()
        {
            //var clients = _db.Clients;
            var clients = _db.Clients;
            return View(clients);
        }

        //Search
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            ViewData["GetClient"] = search;
            var searchQuery = from c in _db.Clients select c;
            if (!string.IsNullOrEmpty(search))
            {
                searchQuery = searchQuery.Where(c => c.Name.Contains(search) || c.Email.Contains(search));
            }
            return View(await searchQuery.AsNoTracking().ToListAsync());
        }

        //GET: Client/Modify/{id}
        /* retrieves modified client from site.js Modify Object Function, checks if client is new or being 
           updated and populates client Add and Edit Modal in Partial View*/
        [HttpGet]
        public IActionResult Modify(int id)
        {
            ClientViewModel viewModel = new ClientViewModel();
            if (id == 0)
            {
                viewModel.Client = new Client();
                viewModel.Title = "Add Client";
            }
            else
            {
                viewModel.Client = _db.Clients.Find(id);
                //viewModel.Client = _db.Clients.Find(id);
                viewModel.Title = "Edit Client";
            }
            return PartialView("_ModifyClientModalPartial", viewModel); ;
        }

        //POST: Client/Modify/{id}
        /* retrieves modified client and sends validation success or fail back to site.js Modify Object Function*/
        [HttpPost]
        public IActionResult Modify(ClientViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.Client.Name))
            {
                var clients = _db.Clients;
                //var clients = _db.Clients;
                if (clients.Any())
                {
                    foreach (Client client in clients)
                    {

                        if (viewModel.Client.Name.ToUpper() == client.Name.ToUpper() && viewModel.Client.Id != client.Id)
                        {
                            ModelState.AddModelError("Client.Name", "There is already a client with this name" +
                                "\r\n Suggestions: Add middle initial or street name to make client easily identifiable");

                        }
                    }
                }
            }

            if (viewModel.Client.Phone != null)
            {
                char firstCharacter = viewModel.Client.Phone[0];
                //check that ph
                if (firstCharacter == '0')
                {
                    ModelState.AddModelError("Client.Phone", "The Phone field is not a valid phone number");
                }

                else if ((viewModel.Client.Phone.Length > 15) || (viewModel.Client.Phone.Length < 7))
                {
                    ModelState.AddModelError("Client.Phone", "Phone number must be between 7 and 15 digits");
                }
            }

            if (ModelState.IsValid)
            {
                if (viewModel.Client.Id == 0)
                {
                    
                    _db.Clients.Add(viewModel.Client);
                    _db.SaveChanges();
                }
                else
                {
                    _db.Clients.Update(viewModel.Client);
                    _db.SaveChanges();
                }
                return Ok(true);
            }
            return PartialView("_ModifyClientModalPartial", viewModel);
        }


        [HttpPost]
        public IActionResult DeletePOST(int? id)
        {
            var selectedClient = _db.Clients.Find(id);
            if (selectedClient == null)
            {
                return NotFound();
            }

            var events = from e in _db.Events where (e.ClientId == id) select e;
            if (events.Any())
            { // all of these event have different id's
                foreach (Event item in events)
                {
                    var eventSchedules = from es in _db.EventSchedules where es.EventId == item.Id select es;
                    if (eventSchedules.Any())
                    {
                        foreach (EventSchedule eventSchedule in eventSchedules)
                        {
                            var schedule = _db.Schedules.Where(s => s.Id == eventSchedule.ScheduleId).FirstOrDefault();
                            _db.Schedules.Remove(schedule);
                            _db.EventSchedules.Remove(eventSchedule);
                        }
                    }
                    _db.Events.Remove(item);
                }
            }
            _db.Clients.Remove(selectedClient);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult Selection(int id)
        {
            var selectedClient = _db.Clients.Find(id);

            return Json(selectedClient.Name);

        }
    }
}

