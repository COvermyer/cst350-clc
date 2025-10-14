using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Difficulty Enum represents the difficulty levels of the game.
    /// 
    /// This value will be used to determine the percentage of the board to
    /// allot bombs. The higher the difficulty, the more bombs will be placed on
    /// 
    /// This value will also be used to determine the score multiplier.
    /// 
    /// Difficulty levels will be recorded in the stats table and will be used 
    /// to separate leaderboards by difficulty.
    /// 
    /// Difficulty Levels:
    /// - Easy: 10% of the board will be bombs
    /// - Medium: 15% of the board will be bombs
    /// - Hard: 20% of the board will be bombs
    /// - Insane: 25% of the board will be bombs
    /// - Impossible: 30% of the board will be bombs
    /// </summary>
    public enum Difficulty
    {
        Easy = 1,
        Medium = 2,
        Hard = 3,
        Insane = 4,
        Impossible = 5
    }
}
