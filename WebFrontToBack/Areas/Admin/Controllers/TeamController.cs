using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;

        public TeamController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<TeamMember> teamMembers = await _context.TeamMembers.ToListAsync();
            return View(teamMembers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamMember teamMembers)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isExists = await _context.TeamMembers.AnyAsync(c =>
            c.FullName.ToLower().Trim() == teamMembers.FullName.ToLower().Trim());

            if (isExists)
            {
                ModelState.AddModelError("FullName", "FullName name already exists");
                return View();
            }
            await _context.TeamMembers.AddAsync(teamMembers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int Id)
        {
            TeamMember? teamMember = _context.TeamMembers.Find(Id);

            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        [HttpPost]
        public IActionResult Update(TeamMember teamMember)
        {
            TeamMember? editedTeamMember = _context.TeamMembers.Find(teamMember.Id);
            if (editedTeamMember == null)
            {
                return NotFound();
            }
            editedTeamMember.FullName = teamMember.FullName;
            _context.TeamMembers.Update(editedTeamMember);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            TeamMember? teamMember = _context.TeamMembers.Find(Id);
            if (teamMember == null)
            {
                return NotFound();
            }
            _context.TeamMembers.Remove(teamMember);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

