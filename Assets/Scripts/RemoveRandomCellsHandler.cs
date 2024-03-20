using System;
using System.Collections.Generic;
using Board;
using UnityEngine.Assertions;

public static class RemoveRandomCellsHandler
{ 
	public static void RemoveRandomCellsFromBoard(IReadOnlyList<Cell> sudokuBoardCells, int cellsToRemove)
	{
		Assert.AreNotEqual(cellsToRemove, 0);

		Random random = new();

		for (int i = 0; i < cellsToRemove; i++)
		{
			int index = 0;
			while (sudokuBoardCells[index].ActualValue == 0)
			{
				index = random.Next(0, sudokuBoardCells.Count - 1);
			}

			sudokuBoardCells[index].SetAsEmpty(sudokuBoardCells[index].ActualValue);
		}
	}
}