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
        public async Task<IActionResult> SearchGuards(string player)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://www.balldontlie.io");

            var response = await client.GetAsync($"/api/v1/players?search={player}");

            var body = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<ApiModel>(body);

            var playersWithPosition = content.Data.Where(x => !string.IsNullOrEmpty(x.Position) && x.Position == "G" || x.Position == "G-F").ToArray();

            content.Data = playersWithPosition;

            return View("SearchResult", content);
        }

        [HttpPost]
        public async Task<IActionResult> SearchForwards(string player)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://www.balldontlie.io");

            var response = await client.GetAsync($"/api/v1/players?search={player}");

            var body = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<ApiModel>(body);

            var playersWithPosition = content.Data.Where(x => !string.IsNullOrEmpty(x.Position) && x.Position == "F").ToArray();
            content.Data = playersWithPosition;

            return View("SearchResult", content);
        }

        [HttpPost]
        public async Task<IActionResult> SearchCenter(string player)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://www.balldontlie.io");

            var response = await client.GetAsync($"/api/v1/players?search={player}");

            var body = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<ApiModel>(body);

            var playersWithPosition = content.Data.Where(x => !string.IsNullOrEmpty(x.Position) && x.Position == "C").ToArray();
            content.Data = playersWithPosition;

            return View("SearchResult", content);
        }

        [HttpPost]
        public async Task<IActionResult> AdvancedSearch(string player, int year)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://www.balldontlie.io");

            var response = await client.GetAsync($"/api/v1/players?search={player}"); // & year   plus change up addPlayer   ****************************************

            var body = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<ApiModel>(body);

            if (year == 0)
            {
                return RedirectToAction("Index");
            }

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
                .FirstOrDefaultAsync(m => m.PlayersOnTeamsId == id);
            if (playersOnTeams == null)
            {
                return NotFound();
            }

            return View(playersOnTeams);
        }

        // Needs more work should everything _context actually be stored in session?
        // https://www.balldontlie.io/api/v1/season_averages?player_ids[]=735&season=2000
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPlayer(string first_name, string last_name, string position, int id)
        {
            var teamID = _session.GetInt32("Team");
            if (position != null)
            {
                if (User.Identity.IsAuthenticated && teamID != null)
                {
                    var team = await _context.BHTeams.Include(x => x.Players).FirstOrDefaultAsync(m => m.BHTeamId == teamID);
                    if (team.Players.Any(x => x.PlayerAPINum == id))
                    {
                        return RedirectToAction("Details", "BHTeams", new { id = teamID });
                    }

                    var year = 2018;

                    var stats = await GetStats(id, year);
                    if (stats == null) return RedirectToAction("Index"); // no stats!

                    var playerOnTeam = new PlayersOnTeams
                    {
                        BHTeamId = (int)teamID,
                        BHTeam = team,
                        PlayerAPINum = id,
                        Name = first_name + " " + last_name,
                        Position = position,
                        Year = year,
                        PPG = stats.pts,
                        Steals = stats.stl,
                        Rebounds = stats.reb
                    };
                    _context.Add(playerOnTeam);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "BHTeams", new { id = teamID });
                }
            }
            
            if (User.Identity.IsAuthenticated && teamID != null)
            {
                var team = await _context.BHTeams.FirstOrDefaultAsync(m => m.BHTeamId == teamID);

                var year = 2018;

                var stats = await GetStats(id, year);
                if (stats == null) return RedirectToAction("Index"); // no stats!

                var playerOnTeam = new PlayersOnTeams
                {
                    BHTeamId = (int)teamID,
                    BHTeam = team,
                    PlayerAPINum = id,
                    Name = first_name + " " + last_name,
                    Position = position,
                    Year = year,
                    PPG = stats.pts,
                    Steals = stats.stl,
                    Rebounds = stats.reb
                };

                _context.Add(playerOnTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "BHTeams", new { id = teamID });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePlayer(int id)
        {
            var teamID = _session.GetInt32("Team");

            var player = await _context.PlayersOnTeams.FindAsync(id);
            _context.PlayersOnTeams.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "BHTeams", new { id = teamID });
        }

        /*            Change this up and allow for advanced search     store year and no need for position parameter************************************************
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLegendaryPlayer(string first_name, string last_name, int year, int id)
        {
            var teamID = _session.GetInt32("Team");
            if (position != null)
            {
                if (User.Identity.IsAuthenticated && teamID != null)
                {
                    var team = await _context.BHTeams.Include(x => x.Players).FirstOrDefaultAsync(m => m.BHTeamId == teamID);
                    if (team.Players.Any(x => x.PlayerAPINum == id))
                    {
                        return RedirectToAction("Details", "BHTeams", new { id = teamID });
                    }

                    var year = 2018;

                    var stats = await GetStats(id, year);
                    if (stats == null) return RedirectToAction("Index"); // no stats!

                    var playerOnTeam = new PlayersOnTeams
                    {
                        BHTeamId = (int)teamID,
                        BHTeam = team,
                        PlayerAPINum = id,
                        Name = first_name + " " + last_name,
                        Position = position,
                        Year = year,
                        PPG = stats.pts,
                        Steals = stats.stl,
                        Rebounds = stats.reb
                    };

                    _context.Add(playerOnTeam);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "BHTeams", new { id = teamID });
                }
            }

            if (User.Identity.IsAuthenticated && teamID != null)
            {
                var team = await _context.BHTeams.FirstOrDefaultAsync(m => m.BHTeamId == teamID);

                var year = 2018;

                var stats = await GetStats(id, year);
                if (stats == null) return RedirectToAction("Index"); // no stats!

                var playerOnTeam = new PlayersOnTeams
                {
                    BHTeamId = (int)teamID,
                    BHTeam = team,
                    PlayerAPINum = id,
                    Name = first_name + " " + last_name,
                    Position = position,
                    Year = year,
                    PPG = stats.pts,
                    Steals = stats.stl,
                    Rebounds = stats.reb
                };

                _context.Add(playerOnTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "BHTeams", new { id = teamID });

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }*/

        // GET: PlayersOnTeams/Create
        public IActionResult Create()
        {
            ViewData["BHTeamId"] = new SelectList(_context.BHTeams, "BHTeamId", "BHTeamId");
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

        public async Task<Stats> GetStats(int id, int year)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://www.balldontlie.io");

            var response = await client.GetAsync($"/api/v1/season_averages?player_ids[]={id}&season={year}");
            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<StatsReturn>(body);
            if (content.data.Length == 0) return null;
            return content.data[0];
        }
    }
}
