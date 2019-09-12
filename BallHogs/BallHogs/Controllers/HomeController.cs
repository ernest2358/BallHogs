using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BallHogs.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BallHogs.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DataContext _context;
        private readonly ISession _session;



        public HomeController(IHttpClientFactory httpClientFactory, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.LoggedIn = User.Identity.IsAuthenticated;
            ViewBag.UID = User.Identity.Name;

            if (User.Identity.IsAuthenticated)
            {
                var manager = await _context.Managers.FirstOrDefaultAsync(m => m.UserName == User.Identity.Name);
                if(manager == null)
                {
                    manager = new Manager(User.Identity.Name);
                    _context.Add(manager);
                    await _context.SaveChangesAsync();
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string player)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://www.balldontlie.io");

            var response = await client.GetAsync($"/api/v1/players?search={player}");

            var body = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<ApiModel>(body);
            var playersWithPosition = content.Data.Where(x => !string.IsNullOrEmpty(x.Position)).ToArray();
            content.Data = playersWithPosition;

            return View("SearchResult", content);
        }
        
        public async Task<IActionResult> LetsBall()
        {
            var allTeams = await _context.BHTeams.ToListAsync();
            var fullTeams = new List<BHTeam>();
            foreach(var team in allTeams)
            {
                var allPlayers = await _context.PlayersOnTeams.Where(m => m.BHTeamId == team.BHTeamId).ToListAsync();
                if (allPlayers.Count == 5) fullTeams.Add(team);
            }
            return View(fullTeams);
        }

        [HttpPost]
        public async Task<IActionResult> LetsBall(int homeId, int awayId, int? games)
        {
            var home = await _context.BHTeams.FirstOrDefaultAsync(m => m.BHTeamId == homeId);
            var away = await _context.BHTeams.FirstOrDefaultAsync(m => m.BHTeamId == awayId);

            var homePlayers = await _context.PlayersOnTeams.Where(m => m.BHTeamId == homeId).ToListAsync();
            var awayPlayers = await _context.PlayersOnTeams.Where(m => m.BHTeamId == awayId).ToListAsync();

            foreach(var player in homePlayers)
            {
                home.Players.Add(player);
            }
            foreach(var player in awayPlayers)
            {
                away.Players.Add(player);
            }

            var series = new Series(home, away, games);
            return View("Results", series);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
