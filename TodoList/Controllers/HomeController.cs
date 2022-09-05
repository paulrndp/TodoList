using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TodoList.Data;
using TodoList.Models;
using TodoList.DAO;
namespace TodoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetAll()
        {
            List<TaskTbl> _list = new TaskDAO(_db).GetAll();
            return Json(_list);
        }
        public JsonResult GetById(int id)
        {
            TaskTbl _list = new TaskDAO(_db).GetById(id);     
            return Json(_list);
        }
        public JsonResult Save(TaskTbl obj)
        {
            string _list = new TaskDAO(_db).Save(obj);
            return Json(_list);
        }
        public JsonResult Remove(int id)
        {
            string _list = new TaskDAO(_db).Remove(id);
            return Json(_list);
        }
        
    }
}