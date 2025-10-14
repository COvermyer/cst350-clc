using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// GameState enum represents the current state of the game.This enum
    /// is used to determine the next steps in the game flow.
    /// 
    /// Game can only be in three states,
    /// - Continue: Game is still ongoing
    /// - Lost: Player has lost the game
    /// - Won: Player has won the game
    /// </summary>
    public enum GameState
    {
        Continue,
        Lost,
        Won
    }
}
