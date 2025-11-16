using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
	public class BoardSerializer
	{
		public static string Serialize(Board board)
		{
			var sb = new StringBuilder(); // will be used to serialize a game down to a string
			sb.Append("MS_SAVE|"); // Will be used to determine validity of save
			string difficulty = "";
			sb.Append($"D:{(int)board.DifficultyLevel}|R:{board.RewardsRemaining}|M:{board.Mistakes}|Start%{board.StartTime}");

			for (int row = 0; row < board.Size; row++)
			{
                sb.Append("|");
                for (int col = 0; col < board.Size; col++)
				{
					var cell = board.Cells[row, col]; // get the cell
					char ch;

					// determine which char to add - Number of bomb neighbors will be dyunamically calculated on deserialization

					if (cell.IsBomb && !cell.IsFlagged) ch = 'B'; // unflagged bomb
					else if (cell.IsBomb && cell.IsFlagged) ch = 'X'; // correctly flagged bomb
					else if (cell.IsFlagged) ch = 'F'; // incorrect flag
					else if (cell.HasSpecialReward) ch = 'R'; // reward
					else if (cell.IsVisited) ch = 'V'; // visited cell
					else ch = 'U'; // unvisited cell
					
					sb.Append(ch);
				}
			}

			return sb.ToString();
		}

		public static Board Deserialize(string saveData)
		{
			string[] data = saveData.Split("|");
			int lineNum = 0;

			if (!data[lineNum++].Equals("MS_SAVE")) // Check that this is a valid save string
				return null; // if not, return null
			 
			Difficulty boardDifficulty = (Difficulty)(int.Parse(data[lineNum++].Split(":")[1]));
			int boardRewards = int.Parse(data[lineNum++].Split(":")[1]);
			int boardMistakes = int.Parse(data[lineNum++].Split(":")[1]);
			DateTime startTime = DateTime.Parse(data[lineNum++].Split("%")[1]);
			int boardSize = data[lineNum].Length; // lineNum is at the first line of the board - calculate size from length

			Board newBoard = new Board(boardSize);
			newBoard.RewardsRemaining = boardRewards;
			newBoard.Mistakes = boardMistakes;
			newBoard.StartTime = startTime;
			newBoard.DifficultyLevel = boardDifficulty;

			int bombCount = 0;
			for (int row = 0; row < boardSize; row++)
			{
				for (int col = 0; col < boardSize; col++)
				{
					char cellType = data[lineNum][col];
					Cell current = new Cell(row, col);

					switch (cellType)
					{
						case 'B': // unflagged bomb
							bombCount++;
							current.IsVisited = false;
							current.IsBomb = true;
							break;
						case 'X': // correctly flagged bomb
							current.IsVisited = false;
							current.IsBomb = true;
							current.IsFlagged = true;
							break;
						case 'F': // incorred flagged cell
							current.IsVisited = false;
							current.IsFlagged = true;
							break;
						case 'V': // visited cell
							current.IsVisited = true;
							break;
						case 'R': // has reward
							current.IsVisited = false;
							current.HasSpecialReward = true;
							break;
						case 'U': // not technically needed
							current.IsVisited = false;
							break;
						default: // unknown case, ignore
							break;
					}

					newBoard.Cells[row, col] = current;
				}
				lineNum++;
			}

			newBoard.TotalBombs = bombCount;
			newBoard.CountNeighboringBombs();
			return newBoard;

		}
	}
}
