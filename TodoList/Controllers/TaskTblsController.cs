using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class TaskTblsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskTblsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TaskTbls
        public async Task<IActionResult> Index()
        {
              return _context.taskTbls != null ? 
                          View(await _context.taskTbls.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.taskTbls'  is null.");
        }

        // GET: TaskTbls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.taskTbls == null)
            {
                return NotFound();
            }

            var taskTbl = await _context.taskTbls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskTbl == null)
            {
                return NotFound();
            }

            return View(taskTbl);
        }

        // GET: TaskTbls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskTbls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Task,Status,Date")] TaskTbl taskTbl)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskTbl);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskTbl);
        }

        // GET: TaskTbls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.taskTbls == null)
            {
                return NotFound();
            }

            var taskTbl = await _context.taskTbls.FindAsync(id);
            if (taskTbl == null)
            {
                return NotFound();
            }
            return View(taskTbl);
        }

        // POST: TaskTbls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Task,Status,Date")] TaskTbl taskTbl)
        {
            if (id != taskTbl.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskTbl);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskTblExists(taskTbl.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskTbl);
        }

        // GET: TaskTbls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.taskTbls == null)
            {
                return NotFound();
            }

            var taskTbl = await _context.taskTbls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskTbl == null)
            {
                return NotFound();
            }

            return View(taskTbl);
        }

        // POST: TaskTbls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.taskTbls == null)
            {
                return Problem("Entity set 'ApplicationDbContext.taskTbls'  is null.");
            }
            var taskTbl = await _context.taskTbls.FindAsync(id);
            if (taskTbl != null)
            {
                _context.taskTbls.Remove(taskTbl);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskTblExists(int id)
        {
          return (_context.taskTbls?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
