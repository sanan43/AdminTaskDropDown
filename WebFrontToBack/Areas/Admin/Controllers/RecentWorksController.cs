using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RecentWorksController : Controller
    {
        private readonly AppDbContext _Context;

        public RecentWorksController(AppDbContext context)
        {
            _Context = context;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<RecentWorks> recentWorks =await _Context.RecentWorks.ToListAsync();
            return View(recentWorks);
        }
        [HttpGet]
         public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RecentWorks recentWorks)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool IsExists=await _Context.RecentWorks.AnyAsync(r=> r.Name.ToLower().Trim()==recentWorks.Name.ToLower().Trim());
            if (IsExists)
            {
                ModelState.AddModelError("Name", "Name already exists");
                return View();
            }
            await _Context.RecentWorks.AddAsync(recentWorks);
            await _Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int Id)
        {
            RecentWorks? recentWorks = _Context.RecentWorks.Find(Id);
            if (recentWorks == null)
            {
                return NotFound();
            }
            return View(recentWorks);
        }
        [HttpPost]
        public IActionResult Update(RecentWorks recentWorks)
        {
            RecentWorks? editedRecentWorks = _Context.RecentWorks.Find(recentWorks.Id);
            if (editedRecentWorks == null)
            {
                return NotFound();
            }
            editedRecentWorks.Name = recentWorks.Name;
            _Context.RecentWorks.Update(editedRecentWorks);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            RecentWorks? recentWorks = _Context.RecentWorks.Find(id);
            if (recentWorks==null)
            {
                return NotFound();
            }
            _Context.RecentWorks.Remove(recentWorks);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
