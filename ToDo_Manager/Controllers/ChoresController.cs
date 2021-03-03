using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDo_Manager.Models;

namespace ToDo_Manager.Controllers
{
    public class ChoresController : Controller
    {
        private readonly ToDoDbContext _context;

        public ChoresController(ToDoDbContext context)
        {
            _context = context;
        }

        // GET: Chores
        public IActionResult Index()
        {
            //get a list of chores where chore.id == 
            List<AspNetUsers> userList = _context.AspNetUsers.ToList();
            AspNetUsers currentUser = GetUser(userList);
            return View(_context.Chore.Where(x => x.UserId == currentUser.Id).ToList());
        }

        // GET: Chores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chore = await _context.Chore
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chore == null)
            {
                return NotFound();
            }

            return View(chore);
        }

        // GET: Chores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create(Chore chore)
        {
            List<AspNetUsers> userList = _context.AspNetUsers.ToList();
            AspNetUsers currentUser = GetUser(userList);
            chore.UserId = currentUser.Id;
            if (ModelState.IsValid)
            {
                _context.Add(chore);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", chore.UserId);
            return View(chore);
        }

        // GET: Chores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chore = await _context.Chore.FindAsync(id);
            if (chore == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", chore.UserId);
            return View(chore);
        }

        // POST: Chores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,CompletionStatus,UserId")] Chore chore)
        {
            if (id != chore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChoreExists(chore.Id))
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
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", chore.UserId);
            return View(chore);
        }

        // GET: Chores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chore = await _context.Chore
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chore == null)
            {
                return NotFound();
            }

            return View(chore);
        }

        // POST: Chores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chore = await _context.Chore.FindAsync(id);
            _context.Chore.Remove(chore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChoreExists(int id)
        {
            return _context.Chore.Any(e => e.Id == id);
        }
        public AspNetUsers GetUser(List<AspNetUsers> userList)
        {
            string username = User.Identity.Name;
            foreach (AspNetUsers member in userList)
            {
                if (member.UserName == username)
                {
                    return member;
                }
            }
           
                return null;
            
        }
    }
}
