namespace MineSweeper
{
    public class Cell
    {
        // Cell Properties

        /// <summary>
        /// Index of Row in the Board
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Index of Comlumn in the Board
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Flag to indicate if the cell has been visited by the player
        /// </summary>
        public bool IsVisited { get; set; }

        /// <summary>
        /// Flag to indicate if the cell contains a bomb
        /// </summary>
        public bool IsBomb { get; set; }
        
        /// <summary>
        /// Flag to indicate if the cell is flagged by the player
        /// </summary>
        public bool IsFlagged { get; set; }

        /// <summary>
        /// Number of cells neighboring this cell that contain bombs
        /// </summary>
        public int NumberOfBombNeighbors { get; set; }

        /// <summary>
        /// Flag to indicate if the cell contains a special reward
        /// </summary>
        public bool HasSpecialReward { get; set; }

        /// <summary>
        /// Default constructor. All properties are initialized as 0 or false.
        /// </summary>
        public Cell()
        {
            this.Row = 0;
            this.Column = 0;
            this.IsVisited = false;
            this.IsBomb = false;
            this.IsFlagged = false;
            this.NumberOfBombNeighbors = 0;
            this.HasSpecialReward = false;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other">Other Cell object to copy</param>
        public Cell(Cell other)
        {
            this.Row = other.Row;
            this.Column = other.Column;
            this.IsVisited = other.IsVisited;
            this.IsBomb = other.IsBomb;
            this.IsFlagged = other.IsFlagged;
            this.NumberOfBombNeighbors = other.NumberOfBombNeighbors;
            this.HasSpecialReward = other.HasSpecialReward;
        }

        /// <summary>
        /// Parameterized constructor to set row and column
        /// </summary>
        /// <param name="row">The row of the Cell</param>
        /// <param name="column">The column of the cell</param>
        public Cell(int row, int column)
        {
            this.Row = row;
            this.Column = column;
            this.IsVisited = false;
            this.IsBomb = false;
            this.IsFlagged = false;
            this.NumberOfBombNeighbors = 0;
            this.HasSpecialReward = false;
        }
    }
}
