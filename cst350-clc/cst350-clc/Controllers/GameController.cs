using cst350_clc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace cst350_clc.Controllers
{
    /// <summary>
    /// GameController will  controll all minesweeper game functions
    /// </summary>
    public class GameController : Controller
    {
        ///GOALS:
        /// - Implement Game Logic
        /// - Implement Game Save (as a file?)
        /// - Implement Game Loading

        /// <summary>
        /// Index will route to main game screen
        /// </summary>
        /// <returns></returns>
        [SessionCheckFilter]
        public IActionResult Index()
        {
            return View();
        }
    }
}
