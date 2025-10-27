using System;
using Microsoft.AspNetCore.Http;
using cst350_clc.Models.Scores;
using MineSweeper;
using cst350_clc.Models.User;
using Microsoft.AspNetCore.Razor.Language;

namespace cst350_clc.Controllers
{
    // FIXME: Move into the HomeController
    public partial class GameController
    {
        private const string SESSION_START_TICKS = "GameStartUtcTicks";
        private const string SESSION_DIFFICULTY = "Difficulty";
        private readonly ScoreDAO _scores = new ScoreDAO();
       
        public void RegisterGameStart(int difficulty)
        {
            
            HttpContext.Session.SetString(SESSION_DIFFICULTY, difficulty.ToString());
            HttpContext.Session.SetString(SESSION_START_TICKS, DateTime.UtcNow.Ticks.ToString());
        }

       
        public void TrySaveWin(Board board)
        {
            var gameState = board.DetermineGameState();
            if (gameState != GameState.Won) return; // if the player hasn't won, return

            int timeSec = (int)(board.EndTime - board.StartTime).TotalSeconds; // simplify, board tracks time already

            int difficulty = (int)board.DifficultyLevel; // can cast an int from enum
            string username = HttpContext.Session.GetString("Username") ?? "GUEST"; // User is known to be logged in due to SessionCheckFilter
            int score = Board.CalculateScore(board); // Board already calculates score

            // FIXME: Remove EnsureSchema() method and migrate schema properly to DDL
            try { _scores.EnsureSchema(); } catch { /* avoid impacting gameplay */ }

            var best = _scores.GetPersonalBest(username, difficulty);
            if (best == null || score > best.Score)
            {
                _scores.Insert(new ScoreModel
                {
                    Username = username,
                    Difficulty = difficulty,
                    Score = score,
                    TimeTaken = timeSec,
                    PlayedAt = DateTime.UtcNow
                });
            }
        }
    }
}
