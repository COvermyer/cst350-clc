using System;

namespace cst350_clc.Models.Scores
{
  
    public class ScoreModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;   
        public int Difficulty { get; set; }                    
        public int Score { get; set; }                       
        public int TimeTaken { get; set; }                    
        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
    }
}
