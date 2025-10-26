using System;
using Microsoft.AspNetCore.Http;
using cst350_clc.Models.Scores;
using MineSweeper;

namespace cst350_clc.Controllers
{
 
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
            var stateName = board.DetermineGameState().ToString();
            if (stateName.IndexOf("win", StringComparison.OrdinalIgnoreCase) < 0) return;

            int timeSec = 0;
            var ticksStr = HttpContext.Session.GetString(SESSION_START_TICKS);
            if (long.TryParse(ticksStr, out var ticks))
            {
                timeSec = (int)Math.Max(0, (DateTime.UtcNow - new DateTime(ticks, DateTimeKind.Utc)).TotalSeconds);
            }

            int difficulty = 0;
            int.TryParse(HttpContext.Session.GetString(SESSION_DIFFICULTY), out difficulty);

            string username = User?.Identity?.IsAuthenticated == true
                ? (User.Identity?.Name ?? "")
                : (HttpContext.Session.GetString("Username") ?? "");

            int score = ComputeScore(board, timeSec);

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

       
        private static int ComputeScore(Board board, int timeSeconds)
        {
            int safeVisited = 0;
            foreach (var cell in board.Cells)
                if (cell.IsVisited && !cell.IsBomb) safeVisited++;

            int baseScore = 500 + safeVisited * 10;   // win bonus + progress
            int timePenalty = Math.Max(0, timeSeconds); // faster is better
            return Math.Max(0, baseScore - timePenalty);
        }
    }
}
