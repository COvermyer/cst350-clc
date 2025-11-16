// cst350-clc/Controllers/GameController.cs
using MineSweeper;
using cst350_clc.Filters;
using Microsoft.AspNetCore.Mvc;
using cst350_clc.Models.GameSave;
using cst350_clc.Services.GameSaveService;
using System.Diagnostics;

namespace cst350_clc.Controllers
{

    public partial class GameController : Controller
    {
        private readonly IGameSaveService _gameSaveService;
        static Board gameBoard = new Board(10, Difficulty.Easy);

        public GameController(IGameSaveService gameSaveService)
        {
            this._gameSaveService = gameSaveService;
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

        [SessionCheckFilter]
        public IActionResult SaveGame()
        {
            int userId = (int)HttpContext.Session.GetInt32("User");

            string boardSerialization = BoardSerializer.Serialize(gameBoard);
            GameSaveModel gameSaveModel = new GameSaveModel
            {
                UserId = userId,
                DateSaved = DateTime.Now,
                SaveData = boardSerialization
            };
            _gameSaveService.AddGameSave(gameSaveModel);
            gameBoard = new Board(10, Difficulty.Easy); // reset board to default

            return RedirectToAction("Index", "Home");
        }

        [SessionCheckFilter]
        public async Task<IActionResult> LoadGame(int gameId)
        {
            GameSaveModel save = await _gameSaveService.GetGameSaveById(gameId);
            gameBoard = BoardSerializer.Deserialize(save.SaveData);
            
            // TESTONLY
            //var state = gameBoard.DetermineGameState();
            //System.Diagnostics.Debug.WriteLine(state.ToString());

            return RedirectToAction("Index");
        }

        [SessionCheckFilter]
        public async Task<IActionResult> ShowSavedGames(int userId)
        {
            IEnumerable<GameSaveModel> saves = await _gameSaveService.GetGameSavesByUserId(userId);
            return View(saves);
        }

        public async Task<IActionResult> DeleteSavedGame(int gameId, int userId)
        {
            await _gameSaveService.DeleteGameSave(gameId);
            return RedirectToAction("ShowSavedGames", userId);
        }
    }
}
