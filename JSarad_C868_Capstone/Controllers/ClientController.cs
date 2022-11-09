using Microsoft.AspNetCore.Mvc;
using JSarad_C868_Capstone.Models;
using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace JSarad_C868_Capstone.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        
        public Client SelectedClient { get; set; }
        private readonly AppDbContext _db;

        public ClientController(AppDbContext db)
        {
            _db = db;
        }
        
        //retrieves clients from database and populates client table
        public IActionResult Index()
        {
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
            Client client;
            if (id == 0)
            {
                client = new Client();
            }
            else
            {
                client= _db.Clients.Find(id);
               
            }
            return PartialView("_ModifyClientModalPartial", client); ;
        }

        //POST: Client/Modify/{Client}
        /* retrieves modified client and sends validation success or fail back to site.js Modify Object Function*/
        [HttpPost]
        public IActionResult Modify(Client client)
        {
            if (ModelState.IsValid)
            {
                if (client.Id == 0)
                {
                    //Uodate your event date entity
                    //Event ev = new Event();
                    //ev.EventStart = 

                    _db.Clients.Add(client);
                    _db.SaveChanges();
                }
                else
                {
                    _db.Clients.Update(client);
                    _db.SaveChanges();
                }
                return Ok(true);
            }
            return PartialView("_ModifyClientModalPartial", client);
        }


        [HttpPost]
        public IActionResult DeletePOST(int? id)
        {
            var selectedClient = _db.Clients.Find(id);
            if (selectedClient == null)
            {
                return NotFound();
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

