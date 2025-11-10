// cst350-clc/Controllers/GameController.cs
using MineSweeper;
using cst350_clc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace cst350_clc.Controllers
{
    
    public partial class GameController : Controller
    {
        static Board gameBoard = new Board(10, Difficulty.Easy);

        public GameController()
        {
        }

        [SessionCheckFilter]
        public IActionResult Index()
        {
            return View(gameBoard);
        }

        [HttpPost]
        public IActionResult GameCellVisit(int row, int col)
        {
            if (gameBoard.DetermineGameState() != GameState.Continue)
                return PartialView("_GameBoard", gameBoard); // Ignore any additional calls if the game is over

            gameBoard.FloodFill(row, col);

            TrySaveWin(gameBoard);
            return PartialView("_GameBoard", gameBoard);
        }

        [HttpPost]
        public IActionResult GameCellFlag(int row, int col)
        {
            if (gameBoard.DetermineGameState() != GameState.Continue)
                return PartialView("_GameBoard", gameBoard); // Ignore any additional calls if the game is over

            gameBoard.FlagCell(row, col);
            TrySaveWin(gameBoard);

            return PartialView("_GameBoard", gameBoard);
        }

        public IActionResult NewGame(int size, int difficulty)
        {
            
            RegisterGameStart(difficulty); 

            gameBoard = new Board(size, (Difficulty)difficulty);
            return RedirectToAction("Index");
        }
    }
}
