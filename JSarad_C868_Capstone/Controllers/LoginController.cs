using Microsoft.AspNetCore.Mvc;
using JSarad_C868_Capstone.Models;
using JSarad_C868_Capstone.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace JSarad_C868_Capstone.Controllers
{
    public class LoginController : Controller
    {

        public static User CurrentUser { get; set; }
        public DataService dataService;
        private readonly AppDbContext _db;

        public LoginController(AppDbContext db)
        {
            _db = db;
            //this.dataService = dataService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            User administrator = new User();
            return View(administrator);

        }

        [HttpPost]
        public async Task<IActionResult> SignIn(User? administrator)
        {
            int adminId;
            var admin = _db.Users.ToList();
           
            if (!ModelState.IsValid) 
            {
                return View("Index", administrator);
            }
            //else if (User) { }
            
            foreach(User user in admin)
            {
                if (user.Username == administrator.Username && user.Password == administrator.Password)
                {
                    adminId = user.Id;
                    DataService.currentUser = _db.Users.Find(adminId);

                    var claims = new List<Claim> 
                    { 
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Email, "admin@catering.com")
                    };
                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);
                    return RedirectToAction("Index", "Event");

                }
            }

            ModelState.AddModelError("Password", "Invalid Username or Password");
            return View("Index", administrator);
        }

        [HttpPost] 
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index");
        }
    }
}

