using JSarad_C868_Capstone.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JSarad_C868_Capstone.Controllers
{
    //[Authorize]
    //[AllowAnonymous]
    public class ValidationController : Controller
    {
        private readonly AppDbContext _db;
        public ValidationController(AppDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        [AcceptVerbs("Get", "Post")]
        public JsonResult IsStartTimeFirst(DateTime StartTime, DateTime EndTime)
        {
            
            if (StartTime > EndTime)
            {
                return Json(data: false);
            }
            return Json(data: true);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}


        
