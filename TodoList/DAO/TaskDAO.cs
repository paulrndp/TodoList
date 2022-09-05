using TodoList.Data;
using TodoList.Models;

namespace TodoList.DAO
{
    public class TaskDAO
    {
        private readonly ApplicationDbContext _db;
        public TaskDAO(ApplicationDbContext db)
        {
            _db = db;
        }
        public List<TaskTbl> GetAll()
        {
            return _db.taskTbls.ToList();
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
                //query.Name = obj.Name;
                //query.Description = obj.Description;
                _db.SaveChanges();
                Result = "pass";
            }
            else
            {
                TaskTbl user = new TaskTbl
                {
                    //Name = obj.Name,
                    //Description = obj.Description,
                };
                _db.taskTbls.Add(user);
                _db.SaveChanges();
                Result = "pass";
            }
            return Result;
        }
    }

}
