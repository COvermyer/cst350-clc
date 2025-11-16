// cst350-clc/Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using cst350_clc.Models.Scores;
using System.Linq;
using MineSweeper;

namespace cst350_clc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ScoreDAO _scores = new ScoreDAO();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try { _scores.EnsureSchema(); } catch { /* don't break homepage if DB is down */ }
            ViewBag.TopScores = _scores.Top(10).ToList();
            return View();
        }

        public IActionResult Privacy() => View();
    }
}
