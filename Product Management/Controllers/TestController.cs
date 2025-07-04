using Microsoft.AspNetCore.Mvc;

namespace Product_Management.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            string timeNowFor = DateTime.Now.ToString();
            ViewBag.showCurTime = timeNowFor;
            return View();
        }

        public string Testpage()
        {
            return "Hello World";
        }
    }
}
