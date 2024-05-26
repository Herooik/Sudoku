using Configs;
using UnityEngine;

namespace Board
{
	public static class BoardHelper
	{
		public static int GetGroupBoxNumber(int row, int column, int subgridRows, int subgridColumns)
		{
			int groupBox = (row / subgridRows) + subgridColumns * (column / subgridColumns);
			return groupBox;
		}

		public static int CalculateIndex(int row, int column, int rows)
		{
			return row * rows + column;
		}

		public static void BuildFromInt(Board board, int[,] grid, SudokuGridConfig sudokuGridConfig)
		{
			int rows = grid.GetLength(0);

			int subgridRows = sudokuGridConfig.SubGridRows;
			int subgridColumns = sudokuGridConfig.SubGridColumns;

			for (int row = 0; row < rows; row++)
			{
				for (int col = 0; col < rows; col++)
				{
					int value = grid[row, col];
					int index = CalculateIndex(row, col, rows);
					int groupBox = GetGroupBoxNumber(row, col, subgridRows, subgridColumns);
					if (value <= 0)
					{
						board.SetCellAsEmpty(index, groupBox, row, col);
					}
					else if (value <= rows)
					{
						board.SetCellAsSolver(index, groupBox, row, col, value);
					}
					else
					{
						Debug.LogError("WTF?!");
					}
				}
			}
		}
	}
}