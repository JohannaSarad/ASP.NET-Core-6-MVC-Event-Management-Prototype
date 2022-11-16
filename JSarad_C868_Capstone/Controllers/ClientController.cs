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
            ClientViewModel viewModel = new ClientViewModel();
            if (id == 0)
            {
                viewModel.Client = new Client();
                viewModel.Title = "Add CLient";
            }
            else
            {
                viewModel.Client = _db.Clients.Find(id);
                viewModel.Title = "Edit Client";
               
            }
            return PartialView("_ModifyClientModalPartial", viewModel); ;
        }

        //POST: Client/Modify/{id}
        /* retrieves modified client and sends validation success or fail back to site.js Modify Object Function*/
        [HttpPost]
        public IActionResult Modify(ClientViewModel viewModel)
        {
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

