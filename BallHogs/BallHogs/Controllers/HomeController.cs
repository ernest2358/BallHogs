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

            return View("SearchResult", content);
        }
             /* store team as an array fixed length of [5]?
              w/in search prompt user 2 guards 2 forward 1 center
        public async Task <IActionResult> CreateATeam()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://www.balldontlie.io");

            var response = await client.GetAsync($"/api/v1/players?search={player}");

            var body = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<ApiModel>(body);

            return View("SearchResult", content);
        } 
        */
        
        public IActionResult LetsBall()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LetsBall(int homeId, int awayId, int? games)
        {
            var home = await _context.BHTeams.FirstOrDefaultAsync(m => m.BHTeamId == homeId);
            var away = await _context.BHTeams.FirstOrDefaultAsync(m => m.BHTeamId == awayId);

            var series = new Series(home, away, games);

            return View("Results", series);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
