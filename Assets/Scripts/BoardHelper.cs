using Configs;
using SudokuBoard.Board;
using UnityEngine;

public static class BoardHelper
{
	public static int GetGroupBoxNumber(int row, int column, int subgridRows, int subgridColumns)
	{
		int groupBox = (row / subgridRows) + subgridColumns * (column / subgridColumns);
		return groupBox;
	}

	public static int CalculateIndex(int row, int rows, int column) //todo flip row with rows parameter
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
				if (value <= 0)
				{
					int index = CalculateIndex(row, rows, col);
					int groupBox = GetGroupBoxNumber(row, col, subgridRows, subgridColumns);
					board.SetCellAsEmpty(index, groupBox, row, col);
				}
				else if (value <= rows)
				{
					int index = CalculateIndex(row, rows, col);
					int groupBox = GetGroupBoxNumber(row, col, subgridRows, subgridColumns);
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