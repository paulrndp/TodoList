using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TodoList.Data;
using TodoList.Models;
using TodoList.DAO;
using TodoList.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace TodoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHubContext<TaskHub> _signalrHub;


        public HomeController(ApplicationDbContext db, IHubContext<TaskHub> signalrHub)
        {
            _db = db;
            _signalrHub = signalrHub;
           
        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetPending()
        {
            List<TaskTbl> _list = new TaskDAO(_db, _signalrHub).GetPending();
            return Json(_list);
        }        
        public JsonResult GetActive()
        {
            List<TaskTbl> _list = new TaskDAO(_db, _signalrHub).GetActive();
            return Json(_list);
        }        
        public JsonResult GetDone()
        {
            List<TaskTbl> _list = new TaskDAO(_db, _signalrHub).GetDone();
            return Json(_list);
        }
        public JsonResult GetById(int id)
        {
            TaskTbl _list = new TaskDAO(_db, _signalrHub).GetById(id);     
            return Json(_list);
        }
        public JsonResult Save(TaskTbl obj)
        {
            string _list = new TaskDAO(_db, _signalrHub).Save(obj);
            return Json(_list);
        }
        public JsonResult Remove(int id)
        {
            string _list = new TaskDAO(_db, _signalrHub).Remove(id);
            return Json(_list);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Item,Category,Price,Qty")] TaskTbl obj)
        {
            if (ModelState.IsValid)
            {
                _db.Add(obj);
                await _db.SaveChangesAsync();
                await _signalrHub.Clients.All.SendAsync("LoadProducts");
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}