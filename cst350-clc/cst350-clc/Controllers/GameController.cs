using MineSweeper;
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

        static Board gameBoard = new Board(10, Difficulty.Easy);

        public GameController()
        {
            //gameBoard.VisitCell(0, 0);
        }


        /// <summary>
        /// Index will route to main game screen
        /// </summary>
        /// <returns></returns>
        [SessionCheckFilter]
        public IActionResult Index()
        {
            
            ViewBag.GameBoard = gameBoard;
            return View();
        }

        [HttpPost]
        public IActionResult GameCellClick(string id)
        {
            var parts = id.Split('_');
            System.Diagnostics.Debug.WriteLine("Clicked cell id: " + id);
            System.Diagnostics.Debug.WriteLine("Parsed parts 0: " + parts[0]);
            System.Diagnostics.Debug.WriteLine("Parsed parts 1: " + parts[1]);
            System.Diagnostics.Debug.WriteLine("Parsed parts 2: " + parts[2]);

            if (parts.Length == 3 && int.TryParse(parts[1], out int row) && int.TryParse(parts[2], out int col))
			{
				gameBoard.FloodFill(row, col);
			}
			else
			{
                // Shoot an error message to the console.
                System.Diagnostics.Debug.WriteLine("Error parsing row and column from id: " + id);
			}

            return RedirectToAction("Index");
        }

        public IActionResult NewGame(int size, int difficulty)
        {
            gameBoard = new Board(size, (Difficulty)difficulty);
            return RedirectToAction("Index");
        }
    }
}
