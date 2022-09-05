using TodoList.Data;
using TodoList.Models;
using TodoList.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace TodoList.DAO
{
    public class TaskDAO
    {
        private readonly ApplicationDbContext _db;
        private readonly IHubContext<TaskHub> _signalrHub;

        public TaskDAO(ApplicationDbContext db, IHubContext<TaskHub> signalrHub)
        {
            _db = db;
            _signalrHub = signalrHub;
        }
        public List<TaskTbl> GetPending()
        {
            return _db.taskTbls.Where(x => x.Status == "pending").Select(all => all).ToList();

        }        
        public List<TaskTbl> GetActive()
        {
            return _db.taskTbls.Where(x => x.Status == "active").Select(all => all).ToList();
        }        
        public List<TaskTbl> GetDone()
        {
            return _db.taskTbls.Where(x => x.Status == "done").Select(all => all).ToList();
        }
        public TaskTbl GetById(int id)
        {
            return _db.taskTbls.FirstOrDefault(x => x.Id == id);

        }
        public string Remove(int id)
        {
            string Result = string.Empty;
            var query = _db.taskTbls.FirstOrDefault(x => x.Id == id);
            if (query != null)
            {
                _db.taskTbls.Remove(query);
                _db.SaveChanges();
                Result = "pass";
            }
            return Result;

        }
        public string Save(TaskTbl obj)
        {
            string Result = string.Empty;
            var query = _db.taskTbls.FirstOrDefault(x => x.Id == obj.Id);
            if (query != null)
            {
                query.Status = obj.Status;
                _db.SaveChanges();
                _signalrHub.Clients.All.SendAsync("LoadProducts");
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
                _db.taskTbls.Add(data);
                _db.SaveChanges();
                _signalrHub.Clients.All.SendAsync("LoadProducts");
                Result = "pass";
            }
            return Result;
        }



    }

}
