using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TodoList.Data;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index() // Create
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Task,Status,Date")] TaskTbl taskTbl)
        {
            if (ModelState.IsValid)
            {
                taskTbl.Status = "pending";
                _db.Add(taskTbl);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskTbl);
        }

    }
}