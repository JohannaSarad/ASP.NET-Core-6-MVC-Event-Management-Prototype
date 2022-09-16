using Microsoft.AspNetCore.Mvc;
using JSarad_C868_Capstone.Models;
using JSarad_C868_Capstone.Data;

namespace JSarad_C868_Capstone.Controllers
{
    public class ClientController : Controller
    {
        private readonly AppDbContext _db;

        public ClientController(AppDbContext db)
        {
            _db = db;
            //_db can be used to access database tables (Entity Framework)
        }
        
        //Get /Client
        public IActionResult Index()
        {
            IEnumerable<Client> clientList = _db.Clients;
            return View(clientList);
        }

        //Get /Client/Add
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Client client)
        {
            //adding custom validation to all 
            //if (client.Name == client.Address)
            //{
            //    ModelState.AddModelError("CustomError", "The Client Name exactly match the client email");
            //}
            //applying custom validation only to the name field
            //if (client.Name == "smurf")
            //{
            //    ModelState.AddModelError("name", "Name cannot be smurf");
            //}

            //validation
            if (ModelState.IsValid)
            {
                //adding a client to database
                _db.Clients.Add(client);
               
                _db.SaveChanges();
                //redirecting to the main client page with list of clients
                return RedirectToAction("Index");
                /*if you need to redirect to an action in a different controller you can do so with 
                 *return RedirectToAction("ActionName", "ControllerName")*/
            }
            else
            {
                return View(client);
                //not exacly sure how this works guess it's just returning the view with the same object in it
            }
        }

        //Get /Client/Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var selectedClient = _db.Clients.Find(id);
           
            //check if selectedClient is null
            if (selectedClient == null)
            {
                return NotFound();
            }
            return View(selectedClient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Client client)
        {
            //validation
            if (ModelState.IsValid)
            {
                //updatng a category in the database
                _db.Clients.Update(client);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(client);
            }
        }

        //Get /Client/Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var selectedClient = _db.Clients.Find(id);

            if (selectedClient == null)
            {
                return NotFound();
            }
            return View(selectedClient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}

