using Microsoft.AspNetCore.Mvc;
using JSarad_C868_Capstone.Models;
using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace JSarad_C868_Capstone.Controllers
{
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

//Get /Client
//public IActionResult Index()
//{
//    ClientListViewModel viewModel = new ClientListViewModel();
//    viewModel.ClientList = _db.Clients;
//    viewModel.SelectedId = 0;
//    viewModel.SelectedName = "";

//    return View(viewModel);
//}
//public IActionResult Add()
//{
//    return View();
//}

//[HttpPost]
//[ValidateAntiForgeryToken]
//public IActionResult Add(Client client)
//{
//    //adding custom validation to all 
//    //if (client.Name == client.Address)
//    //{
//    //    ModelState.AddModelError("CustomError", "The Client Name exactly match the client email");
//    //}
//    //applying custom validation only to the name field
//    //if (client.Name == "smurf")
//    //{
//    //    ModelState.AddModelError("name", "Name cannot be smurf");
//    //}

//    //validation
//    if (ModelState.IsValid)
//    {
//        //adding a client to database
//        _db.Clients.Add(client);

//        _db.SaveChanges();
//        //redirecting to the main client page with list of clients
//        return RedirectToAction("Index");
//        /*if you need to redirect to an action in a different controller you can do so with 
//         *return RedirectToAction("ActionName", "ControllerName")*/
//    }
//    else
//    {
//        return View(client);
//    }
//}

////Get /Client/Edit
//public IActionResult Edit(int? id)
//{
//    if (id == null || id == 0)
//    {
//        return NotFound();
//    }
//    var selectedClient = _db.Clients.Find(id);

//    //check if selectedClient is null
//    if (selectedClient == null)
//    {
//        return NotFound();
//    }
//    return View(selectedClient);
//}

//[HttpPost]
//[ValidateAntiForgeryToken]
//public IActionResult Edit(Client client)
//{
//    //validation
//    if (ModelState.IsValid)
//    {
//        //updatng a category in the database
//        _db.Clients.Update(client);
//        _db.SaveChanges();
//        return RedirectToAction("Index");
//    }
//    else
//    {
//        return View(client);
//    }
//}
