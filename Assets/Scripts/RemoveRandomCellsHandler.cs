using System;
using UnityEngine.Assertions;

public static class RemoveRandomCellsHandler
{ 
	public static void RemoveRandomCellsFromBoard(ICell[,] sudokuBoardCells, int cellsToRemove)
	{
		Assert.AreNotEqual(cellsToRemove, 0);

		Random random = new();
		int rows = sudokuBoardCells.GetLength(0);

		for (int i = 0; i < cellsToRemove; i++)
		{
			int row = random.Next(0, rows);
			int column = random.Next(0, rows);
			while (sudokuBoardCells[row, column] is EmptyCell)
			{
				row = random.Next(0, rows);
				column = random.Next(0, rows);
			}

			ICell temp = sudokuBoardCells[row, column];
			sudokuBoardCells[row, column] = new EmptyCell(temp.Index, temp.GroupBox, temp.Row, temp.Column);
		}
	}
}