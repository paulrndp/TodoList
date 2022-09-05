using Microsoft.AspNetCore.Mvc;
using TodoList.Data;
using TodoList.Models;
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
        [HttpGet]
        public IActionResult GetPending()
        {
            var res = _db.taskTbls.Where(x=>x.Status == "pending").Select(all => all).ToList();
            return Ok(res);
        }          
        [HttpGet]
        public IActionResult GetActive()
        {
            var res = _db.taskTbls.Where(x=>x.Status == "active").Select(all => all).ToList();
            return Ok(res);
        }          [HttpGet]
        public IActionResult GetDone()
        {
            var res = _db.taskTbls.Where(x=>x.Status == "done").Select(all => all).ToList();
            return Ok(res);
        }        
        [HttpPost]
        public async Task<IActionResult> Save(TaskTbl obj)
        {
            string Result = string.Empty;
            var query = _db.taskTbls.FirstOrDefault(x=>x.Id == obj.Id);

            if (query != null)
            {
                query.Status = obj.Status;
               await _db.SaveChangesAsync();
               await _signalrHub.Clients.All.SendAsync("LoadData");
                Result = "pass";
            }
            else
            {
                TaskTbl data = new TaskTbl
                {
                    Title = obj.Title,
                    Task = obj.Task,
                    Status = obj.Status,
                    Date = obj.Date,
                };
                _db.Add(obj);
                await _db.SaveChangesAsync();
                await _signalrHub.Clients.All.SendAsync("LoadData");
                Result = "pass";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
        public async Task<IActionResult> Remove(int id)
        {

            var query = await _db.taskTbls.FindAsync(id);
            if (query != null)
            {
                _db.taskTbls.Remove(query);
                await _signalrHub.Clients.All.SendAsync("LoadData");

            }

            await _db.SaveChangesAsync();
            await _signalrHub.Clients.All.SendAsync("LoadData");
            return RedirectToAction(nameof(Index));
        }
    }
}