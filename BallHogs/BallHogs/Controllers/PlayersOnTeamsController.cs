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
            var dataContext = _context.BHTeams.Include(r => r.Players).Where(x => x.ManagerName == User.Identity.Name);
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

        //Needs more work should everything _context actually be stored in session?
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPlayers(string first_name, string last_name, string position, int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = await _context.Managers.FirstOrDefaultAsync(m => m.UserName == User.Identity.Name);
                if (currentUser == null)
                {
                    var userData = new Manager(User.Identity.Name);
                    _context.Add(userData);
                    await _context.SaveChangesAsync();
                    currentUser = userData;
                }

                var player = new Datum
                {
                    First_name = first_name,
                    Last_name = last_name,
                    Position = position,
                    Id = id
                };
                _context.Add(player);
                await _context.SaveChangesAsync();

                //Trying to add this blayer to our collection of plaers in BHteam
                //player = await _context.BHTeams.FirstOrDefaultAsync(m => m.Players == ;

                var team = new PlayersOnTeams();
                team.BHTeamId = currentUser.ManagerID;
                team.PlayersOnTeamsId = player.Id;
                _context.Add(player);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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
