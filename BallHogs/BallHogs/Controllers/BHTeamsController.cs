using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BallHogs.Models;

namespace BallHogs.Controllers
{
    public class BHTeamsController : Controller
    {
        private readonly DataContext _context;

        public BHTeamsController(DataContext context)
        {
            _context = context;
        }

        // GET: BHTeams
        public async Task<IActionResult> Index()
        {
            return View(await _context.BHTeams.ToListAsync());
        }

        // GET: BHTeams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bHTeam = await _context.BHTeams
                .FirstOrDefaultAsync(m => m.BHTeamId == id);
            if (bHTeam == null)
            {
                return NotFound();
            }

            return View(bHTeam);
        }

        // GET: BHTeams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BHTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BHTeamId,TeamName,ManagerName")] BHTeam bHTeam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bHTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bHTeam);
        }

        // GET: BHTeams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bHTeam = await _context.BHTeams.FindAsync(id);
            if (bHTeam == null)
            {
                return NotFound();
            }
            return View(bHTeam);
        }

        // POST: BHTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BHTeamId,TeamName,ManagerName")] BHTeam bHTeam)
        {
            if (id != bHTeam.BHTeamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bHTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BHTeamExists(bHTeam.BHTeamId))
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
            return View(bHTeam);
        }

        // GET: BHTeams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bHTeam = await _context.BHTeams
                .FirstOrDefaultAsync(m => m.BHTeamId == id);
            if (bHTeam == null)
            {
                return NotFound();
            }

            return View(bHTeam);
        }

        // POST: BHTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bHTeam = await _context.BHTeams.FindAsync(id);
            _context.BHTeams.Remove(bHTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BHTeamExists(int id)
        {
            return _context.BHTeams.Any(e => e.BHTeamId == id);
        }
    }
}
