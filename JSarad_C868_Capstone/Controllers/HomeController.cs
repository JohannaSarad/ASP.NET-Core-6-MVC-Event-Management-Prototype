using JSarad_C868_Capstone.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace JSarad_C868_Capstone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string ReturnUrl) //new cookie
        {
            if ((username == "Admin") && (password == "Admin"))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Login");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Redirect(ReturnUrl == null ? "/Secured" : ReturnUrl);
            }
            else
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout() //new cookie
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //To perform the logout of the current user, just call the SignOutAsync() method.This method removes the Authentication Cookie from the browser.
    }
}