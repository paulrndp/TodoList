using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TodoList.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}