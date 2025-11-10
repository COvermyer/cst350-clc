using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Board class represents the game board.
    /// 
    /// This class will maintain the state of the board, including the size,
    /// difficulty, start and end time, and the cells on the board.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Represents the side length of the square board. Board area is always square.
        /// Board area will be calculated as @(Size^2). 
        /// If size is 9, there will be 9 rows, 9 columns, indexed 0-8, for a total of 81 cells.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Time in which the game is started. This value is set when the board is instantiated. Used for recording scores
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Time in which the game is ended. This value is set when the game is won or lost. Used for recording scores.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 2D array of Cell objects representing the game board.
        /// </summary>
        public Cell[,] Cells { get; set; }

        /// <summary>
        /// Difficulty level of the game. This value is set when the board is instantiated.
        /// </summary>
        public Difficulty DifficultyLevel { get; set; }

        /// <summary>
        /// Number of remaining rewards on the board. This value is set when the board is instantiated.
        /// </summary>
        public int RewardsRemaining { get; set; }

        /// <summary>
        /// Total number of bombs on the board. This value is calculated when the board is instantiated.
        /// </summary>
        public int TotalBombs { get; set; }

        /// <summary>
        /// Number of mistakes made by the player. This value is incremented each time the player incorrectly flags a cell.
        /// </summary>
        public int Mistakes { get; set; }

        /// <summary>
        /// Percentage of the board that will be bombs. This value is determined by the difficulty level and calculated at Board instantiation.
        /// This value is not dynamic and will not change during the game. This value is used prior to game start to determine the TotalBombs value.
        /// </summary>
        private decimal BombPercentage { get; set;}

        /// <summary>
        /// Constructor for the Board class. Initializes the board with the specified size and difficulty level.
        /// </summary>
        /// <param name="size">Side length of the board</param>
        /// <param name="difficultyLevel">Difficulty Level the game will be played at</param>
        /// <param name="rewardsRemaining">Optional number of rewards to allot the player at game start</param>
        public Board(int size, Difficulty difficultyLevel, int rewardsRemaining = 0)
        {
            // Constructor must validate all assignments before making them.
            // Size - minimum "playable" board is 2x2, although it definitely isn't fun.
            if (size < 2)
                this.Size = 2;
            else
                this.Size = size;

            // Determine StartTime - set to current time
            this.StartTime = DateTime.Now; // Speed of completion will be used to formulate score.

            // Determine the BombPercentage based on the difficulty level
            this.DifficultyLevel = difficultyLevel;
            this.BombPercentage = DetermineBombPercentage(this.DifficultyLevel);

            // Using the board area @(Size^2) and bomb percentage, determine the total number of bombs
            this.TotalBombs = Convert.ToInt32(Math.Round(this.Size * this.Size * this.BombPercentage, MidpointRounding.AwayFromZero));
            this.TotalBombs = (this.TotalBombs > 0) ? this.TotalBombs : 1; // Ensure at least one bomb is present

            this.Mistakes = 0; // Initialize mistakes to 0

            /// Initialize the board by instantiating the Cells array, placing bombs, placing rewards, and counting neighboring bombs per Cell
            // Instantiate the Cells array
            Cells = InstantiateCellGrid(this.Size);
            // Instantiate bombs on the board
            PlaceBombs();
            // Inform each cell of their neighboring bombs
            CountNeighboringBombs();
            // Place rewards on the board
            PlaceRewards();
        }

        /// <summary>
        /// Visits a cell on the board and determines if a reward was in that cell
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void VisitCell(int row, int col)
        {
            Cells[row, col].IsVisited = true; // Mark the cell as visited

            if (Cells[row, col].IsFlagged) // If the cell was flagged, unflag it
                Cells[row, col].IsFlagged = false; // Unflag the cell (flag is removed when visited regardless of correctness)

            if (Cells[row, col].HasSpecialReward)
                ++RewardsRemaining; // Increment rewards if the cell has a special reward
        }

        /// <summary>
        /// Recursively visit cells adjacent to the selected cell until cells with neighboring bombs are reached.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void FloodFill(int row, int col)
        {
            /// EXIT CONDITIONS
            if (row < 0 || row >= this.Size)
                return; // Row is out of bounds

            if (col < 0 || col >= this.Size)
                return; // Column is out of bounds

            if (Cells[row, col].IsVisited)
                return; // Cell has already been visited

            if (Cells[row, col].IsBomb || Cells[row, col].IsFlagged)
            {
                this.VisitCell(row, col);
                return; // Cell is a bomb or flagged, do not visit (bomb is only possible if the initial cell is a bomb)
            }

            if (Cells[row, col].NumberOfBombNeighbors > 0)
            {
                this.VisitCell(row, col); // Visit the cell and stop recursion if it has neighboring bombs
                return;
            }

            this.VisitCell(row, col); // Visit the cell if no exit conditions were reached

            // RECURSIVE CALLS
            FloodFill(row - 1, col); // Up
            FloodFill(row + 1, col); // Down
            FloodFill(row, col - 1); // Left
            FloodFill(row, col + 1); // Right
            FloodFill(row - 1, col - 1); // Up-Left
            FloodFill(row - 1, col + 1); // Up-Right
            FloodFill(row + 1, col - 1); // Down-Left
            FloodFill(row + 1, col + 1); // Down-Right
        }

        /// <summary>
        /// Flags a cell on the board. If the cell is already flagged, unflags it.
        /// If the flagged cell does not contain a bomb, up the mistakes value by 1.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void FlagCell(int row, int col)
        {
            if (!Cells[row, col].IsVisited) // Cant flag a visited cell
            {
                if (Cells[row, col].IsFlagged) // Cell is already flagged, unflag it
                    Cells[row, col].IsFlagged = false; // Unflag the cell if it is already flagged
                else
                {
                    Cells[row, col].IsFlagged = true; // Flag the cell if it is not already flagged
                    if (!Cells[row, col].IsBomb) // If the flagged cell is not a bomb, increment mistakes
                        ++Mistakes;
                }
            }
        }

        /// <summary>
        /// Uses the special reward at the specified cell, if one exists. Reward acts as a probe and will tell
        /// the user if a bomb is present in a selected cell
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool UseReward(int row, int col)
        {
            if (RewardsRemaining <= 0)
                return false; // No rewards remaining, cannot use reward

            RewardsRemaining--; // Decrement rewards remaining
            return Cells[row, col].IsBomb; // Return true if the cell contains a bomb, false otherwise
        }

        /// <summary>
        /// Determines the current state of the game.
        /// </summary>
        /// <returns></returns>
        public GameState DetermineGameState()
        {
            /// Win Conditions:
            /// if flaggedCellsCount == flaggedBombsCount == TotalBombs, Win (All bombs are correctly flagged)
            /// OR
            /// if visitedCellsCount == (boardArea - TotalBombs), Win (All non-bomb cells have been visited)
            
            /// Lost Conditions:
            /// if bomb is visited, Lose

            /// Continue Conditions:
            /// if flaggedCellsCount != TotalBombs, and
            /// if visitedCellsCount < (boardArea - TotalBombs), continue

            // Cell Counters
            int flaggedCellsCount = 0;
            int flaggedBombsCount = 0;
            int visitedCellsCount = 0;

            // iterate through all cells and count flagged cells, flagged bombs, and visited cells.
            // Check for lose condition (bomb is visited)
            foreach (Cell cell in this.Cells)
            {
                // Check visitation
                if (cell.IsVisited)
                {
                    visitedCellsCount++;
                    if (cell.IsBomb)
                    {
                        EndTime = DateTime.Now; // Set the end time if the game is lost
                        return GameState.Lost; // If a bomb is visited, the game is lost.
                    }
                       
                }

                // Check flags
                if (cell.IsFlagged)
                {
                    flaggedCellsCount++;
                    if (cell.IsBomb)
                        flaggedBombsCount++;
                }
            }

            // Perform checks for win
            // WIN CASE 1: All bombs are correctly flagged
            if (flaggedBombsCount == flaggedCellsCount && flaggedBombsCount == this.TotalBombs)
            {
                EndTime = DateTime.Now; // Set the end time if the game is won
                return GameState.Won;
            }
                

            // WIN CASE 2: All non-bomb cells have been visited
            if (visitedCellsCount == (this.Size * this.Size) - this.TotalBombs)
            {
                EndTime = DateTime.Now; // Set the end time if the game is won
                return GameState.Won;
            }
                

            // If game is not won or lost, it must be continuing.
            return GameState.Continue;
        }

        /// <summary>
        /// Calculates the current score based on the current state of the board.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static int CalculateScore(Board board)
        {
            if (board.DetermineGameState() != GameState.Won)
                return 0; // Score is only calculated if the game is won.

            int totalCells = board.Size * board.Size;

            // determine the score multiplier based on the difficulty level
            double[] difficultyMultipliers = { 1.0, 1.5, 2.0, 2.5, 3.0 }; // Multipliers for Easy, Medium, Hard, Insane, Impossible
            double difficultyMultiplier = difficultyMultipliers[(int)board.DifficultyLevel];

            // Determine board complexity (area * bomb percentage)
            // This gives a higher complexity value to boards that are larger and have more bombs.
            double boardComplexity = (totalCells) * (double)board.BombPercentage;

            // Determine the time factor
            // takes 2 seconds per tile -> minimum 0.2 multiplier
            // Faster => higher score
            double timeTaken = (board.EndTime - board.StartTime).TotalSeconds;
            double timeFactor = Math.Max(0.2, 1.0 - (timeTaken / (totalCells * 2.0)));

            // Determine the mistake penalty
            double accuracyFactor = 1.0 - (board.Mistakes * 0.02); // Each mistake reduces accuracy by 2%
            accuracyFactor = Math.Max(0.5, accuracyFactor); // Minimum accuracy factor is 50%

            double baseScore = boardComplexity * difficultyMultiplier;
            double preBonusScore = baseScore * timeFactor * accuracyFactor;

            // determine bonus for remaining rewards at end of game
            double rewardsBonus = (board.RewardsRemaining * 0.02); // Each remaining reward adds 2% to the score
            rewardsBonus = Math.Min(0.1, rewardsBonus); //Maximum bonus is 10%

            // determine if a perfection bonus is applicable
            double perfectionBonus = (board.Mistakes == 0) ? 0.05 : 0.0; // 5% bonus for no mistakes

            // Calculate final score
            double finalScore = preBonusScore * (1.0 + rewardsBonus + perfectionBonus);

            // return the final score
            return (int)Math.Round(finalScore, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Returns the appropriate BombPercentage based on the Difficulty level.
        /// </summary>
        /// <param name="difficultyLevel"></param>
        /// <returns></returns>
        private decimal DetermineBombPercentage(Difficulty difficultyLevel)
        {
            switch (difficultyLevel)
            {
                case Difficulty.Easy:
                    return 0.10M; // 10% of the board will be bombs on Easy
                case Difficulty.Medium:
                    return 0.15M; // 15% of the board will be bombs on Medium
                case Difficulty.Hard:
                    return 0.20M; // 20% of the board will be bombs on Hard
                case Difficulty.Insane:
                    return 0.25M; // 25% of the board will be bombs on Insane
                case Difficulty.Impossible:
                    return 0.30M; // 30% of the board will be bombs on Impossible
                default: // Should be unreachable, but any unknown value will be treated as Easy
                    return 0.10M;
            }
        }

        /// <summary>
        /// Instantiates the Cell grid based on the Size property of the board.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private Cell[,] InstantiateCellGrid(int size)
        {
            Cell[,] cells = new Cell[size, size]; // create a temp cells array
            // Iterate through the array and instantiate each cell, defining its row and column values
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    cells[row, col] = new Cell()
                    {
                        Row = row,
                        Column = col
                    };
                }
            }
            return cells; // return the temp cells array
        }
    
        /// <summary>
        /// Iterates through each cell and counts how many bombs are neighboring the cell. If the cell itself is a bomb, the value is set to -1
        /// </summary>
        private void CountNeighboringBombs()
        {
            // Iterate through each cell in the grid
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    // Skip if the cell itself is a bomb
                    if (Cells[row, col].IsBomb)
                    {
                        Cells[row, col].NumberOfBombNeighbors = -1; // Indicate that this cell is a bomb
                        continue;
                    }

                    int bombCount = 0;

                    // Check all 8 neighboring cells
                    for (int r = row - 1; r <= row + 1; r++)
                    {
                        for (int c = col - 1; c <= col + 1; c++)
                        {
                            // Skip the cell itself
                            if (r == row && c == col)
                                continue;

                            // Check if the neighbor is within bounds
                            if (r >= 0 && r < Size && c >= 0 && c < Size)
                            {
                                if (Cells[r, c].IsBomb)
                                    bombCount++;
                            }
                        }
                    }

                    // Set the number of neighboring bombs for the current cell
                    Cells[row, col].NumberOfBombNeighbors = bombCount;
                }
            }
        }

        /// <summary>
        /// Utilizes a random number generator to place bombs on the board based on the TotalBombs property.
        /// </summary>
        private void PlaceBombs()
        {
            // Create new instance of Random class
            Random rand = new Random();

            // Iterate until all bombs are placed
            for (int placed = 0; placed < TotalBombs; placed++)
            {
                // pull a random row and column
                int row = rand.Next(0, Size);
                int col = rand.Next(0, Size);

                if (Cells[row, col].IsBomb) // If a bomb is already present, try again
                {
                    placed--; // Decrement placed to try again
                    continue;
                }
                else
                {
                    Cells[row, col].IsBomb = true; // Place a bomb
                }
            }
        }
    
        /// <summary>
        /// Utilizes a random number generator to place special rewards on the board based on the size and DifficultyLevel.
        /// </summary>
        private void PlaceRewards()
        {
            /// Part 1: Calculate how many rewards should be placed based on the board size and difficulty level
            double rewardsPercentage;
            switch (this.DifficultyLevel)
            { // TODO: Balance Me
                case Difficulty.Easy:
                    rewardsPercentage = 0.05; // 5% of the board can be rewards on Easy
                    break;
                case Difficulty.Medium:
                    rewardsPercentage = 0.03; // 3% of the board can be rewards on Medium
                    break;
                case Difficulty.Hard:
                    rewardsPercentage = 0.02; // 2% of the board can be rewards on Hard
                    break;
                case Difficulty.Insane:
                    rewardsPercentage = 0.01; // 1% of the board can be rewards on Insane
                    break;
                case Difficulty.Impossible:
                    rewardsPercentage = 0.0; // No Rewards on Impossible
                    return; // Don't bother trying to place 0 rewards on Impossible difficulty
                default: // Should be unreachable, but any unknown value will be treated as Easy
                    rewardsPercentage = 0.05;
                    break;
            }

            int rewardsToPlace = Convert.ToInt32(Math.Round((this.Size * this.Size) * rewardsPercentage));
            rewardsToPlace = (rewardsToPlace > 0) ? rewardsToPlace : 1; // Ensure at least one reward is present if percentage > 0

            /// Part 2: Place the rewards on the board
            Random rand = new Random(); // Create new instance of Random class
            int row, col;
            for (int placed = 0; placed < rewardsToPlace; placed++)
            {
                row = rand.Next(0, Size);
                col = rand.Next(0, Size);

                if (Cells[row, col].IsBomb)
                    continue; // Cannot place a reward on a bomb, reward is lost (creates variance in number of rewards per game)

                Cells[row, col].HasSpecialReward = true; // Place a reward
            }
        }

        public override string ToString()
        {
            string output = "MINESWEEPER";
            string divider = $"\n{String.Concat(Enumerable.Repeat("+---", this.Size))}+\n";

            // Add the Header Row
            //output += "   "; // Initial padding for the row numbers
            //for (int col = 0; col < this.Size; col++)
            //    output += String.Format("{0, 3}", col);
            output += divider; // Add the divider after the header row

            // Add each row of the board
            for (int row = 0; row < this.Size; row++)
            {
                //// Add the row number
                //output += String.Format("{0, 3}", row);
                for (int col = 0; col < this.Size; col++) // go through each column to add the value
                {
                    output += "|"; // Add the left border of the cell
                    if (Cells[row, col].IsBomb)
                        output += " B "; // Bomb
                    else if (Cells[row, col].HasSpecialReward)
                        output += " R "; // Reward
                    else if (Cells[row, col].NumberOfBombNeighbors == 0)
                        output += " . "; // No neighboring bombs (empty cell)
                    else
                        output += $" {Cells[row, col].NumberOfBombNeighbors} "; // Number of neighboring bombs
                }
                output += "|" + divider; // Close the row with a right border and add the divider
            }

            return output.Trim('\n');
        }
    }
}
