using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BallHogs.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BallHogs.Controllers
{
    public class PlayersOnTeamsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DataContext _context;
        private readonly ISession _session;

        public PlayersOnTeamsController(IHttpClientFactory httpClientFactory, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
            _session = httpContextAccessor.HttpContext.Session;
        }


        // GET: PlayersOnTeams
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.PlayersOnTeams.Include(p => p.BHTeam).Include(p => p.Datum);
            return View(await dataContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string player)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://www.balldontlie.io");

            var response = await client.GetAsync($"/api/v1/players?search={player}");

            var body = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<ApiModel>(body);

            return View("SearchResult", content);
        }

        // GET: PlayersOnTeams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playersOnTeams = await _context.PlayersOnTeams
                .Include(p => p.BHTeam)
                .Include(p => p.Datum)
                .FirstOrDefaultAsync(m => m.PlayersOnTeamsId == id);
            if (playersOnTeams == null)
            {
                return NotFound();
            }

            return View(playersOnTeams);
        }

        // GET: PlayersOnTeams/Create
        public IActionResult Create()
        {
            ViewData["BHTeamId"] = new SelectList(_context.BHTeams, "BHTeamId", "BHTeamId");
            ViewData["DatumId"] = new SelectList(_context.Set<Datum>(), "Id", "Id");
            return View();
        }

        // POST: PlayersOnTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlayersOnTeamsId,DatumId,BHTeamId")] PlayersOnTeams playersOnTeams)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playersOnTeams);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BHTeamId"] = new SelectList(_context.BHTeams, "BHTeamId", "BHTeamId", playersOnTeams.BHTeamId);
            ViewData["DatumId"] = new SelectList(_context.Set<Datum>(), "Id", "Id", playersOnTeams.DatumId);
            return View(playersOnTeams);
        }

        // GET: PlayersOnTeams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playersOnTeams = await _context.PlayersOnTeams.FindAsync(id);
            if (playersOnTeams == null)
            {
                return NotFound();
            }
            ViewData["BHTeamId"] = new SelectList(_context.BHTeams, "BHTeamId", "BHTeamId", playersOnTeams.BHTeamId);
            ViewData["DatumId"] = new SelectList(_context.Set<Datum>(), "Id", "Id", playersOnTeams.DatumId);
            return View(playersOnTeams);
        }

        // POST: PlayersOnTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlayersOnTeamsId,DatumId,BHTeamId")] PlayersOnTeams playersOnTeams)
        {
            if (id != playersOnTeams.PlayersOnTeamsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playersOnTeams);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayersOnTeamsExists(playersOnTeams.PlayersOnTeamsId))
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
            ViewData["BHTeamId"] = new SelectList(_context.BHTeams, "BHTeamId", "BHTeamId", playersOnTeams.BHTeamId);
            ViewData["DatumId"] = new SelectList(_context.Set<Datum>(), "Id", "Id", playersOnTeams.DatumId);
            return View(playersOnTeams);
        }

        // GET: PlayersOnTeams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playersOnTeams = await _context.PlayersOnTeams
                .Include(p => p.BHTeam)
                .Include(p => p.Datum)
                .FirstOrDefaultAsync(m => m.PlayersOnTeamsId == id);
            if (playersOnTeams == null)
            {
                return NotFound();
            }

            return View(playersOnTeams);
        }

        // POST: PlayersOnTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playersOnTeams = await _context.PlayersOnTeams.FindAsync(id);
            _context.PlayersOnTeams.Remove(playersOnTeams);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayersOnTeamsExists(int id)
        {
            return _context.PlayersOnTeams.Any(e => e.PlayersOnTeamsId == id);
        }
    }
}
