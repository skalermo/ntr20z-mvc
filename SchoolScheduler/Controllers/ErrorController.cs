using Microsoft.AspNetCore.Mvc;


namespace SchoolScheduler.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }

}