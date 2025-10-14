using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Class records the results of a completed game.
    /// </summary>
    public class GameRecord
    {
        /// <summary>
        /// The final score from the game
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// The Difficulty level of the game played
        /// </summary>
        public Difficulty DifficultyLevel { get; set; }
        
        /// <summary>
        /// Time taken to complete the game in seconds. If the game is not completed, this value is -1.
        /// </summary>
        public int TimeTaken { get; set; }

        /// <summary>
        /// Default constructor. All properties are initialized to default values.
        /// </summary>
        public GameRecord()
        {
            this.Score = 0;
            this.DifficultyLevel = Difficulty.Easy;
            this.TimeTaken = -1;
        }

        /// <summary>
        /// Parmaterized constructor to create a GameRecord with specified values.
        /// </summary>
        /// <param name="score"></param>
        /// <param name="difficultyLevel"></param>
        /// <param name="state"></param>
        /// <param name="timeTaken"></param>
        public GameRecord(int score, Difficulty difficultyLevel, GameState state, int timeTaken = 0)
        {
            this.Score = score;
            this.DifficultyLevel = difficultyLevel;
            this.TimeTaken = timeTaken;
        }

        /// <summary>
        /// Parameterized constructor to create a GameRecord with specified values. Time taken is calculated from start and end time.
        /// </summary>
        /// <param name="score"></param>
        /// <param name="difficultyLevel"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public GameRecord(int score, Difficulty difficultyLevel, GameState state, DateTime startTime, DateTime endTime)
        {
            this.Score = score;
            this.DifficultyLevel = difficultyLevel;
            this.TimeTaken = (int)(endTime - startTime).TotalSeconds;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public GameRecord(GameRecord other)
        {
            this.Score = other.Score;
            this.DifficultyLevel = other.DifficultyLevel;
            this.TimeTaken = other.TimeTaken;
        }

        /// <summary>
        /// Constructor to generate a GameRecord from a Board object.
        /// </summary>
        /// <param name="board"></param>
        public GameRecord(Board board)
        {
            this.Score = Board.CalculateScore(board);
            this.DifficultyLevel = board.DifficultyLevel;
            
            // If the score is 0, the game was not won, so set TimeTaken to -1.
            if (this.Score != 0)
                this.TimeTaken = (int)(board.EndTime - board.StartTime).TotalSeconds;
            else
                this.TimeTaken = -1;
        }
    }
}
