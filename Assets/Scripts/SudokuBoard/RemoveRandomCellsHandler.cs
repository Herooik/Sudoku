﻿using System;
using UnityEngine.Assertions;

namespace SudokuBoard
{
	public static class RemoveRandomCellsHandler
	{ 
		public static void RemoveRandomCellsFromBoard(Board.Board sudokuBoard, int cellsToRemove, Action<int, int, int, int> setCellAsEmpty)
		{
			Assert.AreNotEqual(cellsToRemove, 0);

			Random random = new();
			int rows = sudokuBoard.GetRowsLength();

			for (int i = 0; i < cellsToRemove; i++)
			{
				int row = random.Next(0, rows);
				int column = random.Next(0, rows);
				ICell cellToRemove = sudokuBoard.GetCell(row, column); 
				while (cellToRemove.IsEmpty)
				{
					row = random.Next(0, rows);
					column = random.Next(0, rows);
					cellToRemove = sudokuBoard.GetCell(row, column);
				}

				setCellAsEmpty?.Invoke(cellToRemove.Index, cellToRemove.GroupBox, cellToRemove.Row, cellToRemove.Column);
			}
		}
	}
}