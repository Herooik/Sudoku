using System.Collections.Generic;
using System.Linq;
using Configs;
using SudokuBoard.BoardGenerator;
using SudokuBoard.Solver;
using UnityEngine;

namespace SudokuBoard.Board
{
	public class SudokuBoard
	{
		public readonly ICell[,] CellsArray;

		private readonly IBoardSolver _boardSolver;
		private readonly SudokuGridConfig _sudokuGridConfig;

		public SudokuBoard(SudokuGridConfig sudokuGridConfig, IBoardSolver boardSolver)
		{
			_sudokuGridConfig = sudokuGridConfig;
			_boardSolver = boardSolver;

			CellsArray = new ICell[sudokuGridConfig.Rows, sudokuGridConfig.Rows];
		}

		public void GenerateNewBoard(int cellsToRemove)
		{
			IBoardGenerator boardGenerator = new RandomBoardGenerator(_sudokuGridConfig, _boardSolver, CanPlaceValue, IsFullFilled);
			boardGenerator.Generate(CellsArray);

			// todo move to other place
			RemoveRandomCellsHandler.RemoveRandomCellsFromBoard(CellsArray, cellsToRemove, SetCellAsEmpty);
		}

		public bool IsFullFilled()
		{
			foreach (ICell cell in CellsArray)
			{
				if (cell.IsEmpty)
					return false;
			}
			return true;
		}

		public bool IsValueReachMaxOutUsed(int value) //todo CHANGE NAME
		{
			int count = 0;
			foreach (ICell cell in CellsArray)
			{
				if (cell.IsFilledGood && cell.Number == value)
				{
					count++;
				}
			}
			return count == CellsArray.GetRowsLength();
		}

		public void PlaceValue(int value, int cellRow, int cellColumn)
		{
			ICell targetCell = CellsArray[cellRow, cellColumn];
			if (targetCell is CellForUser cellForUser)
			{
				cellForUser.FillCell(value);
			}
			else
			{
				Debug.LogWarning("CAN'T PLACE NUMBER ON GENERATED CELL");
			}
		}

		public void CleanCell(int cellRow, int cellColumn)
		{
			ICell targetCell = CellsArray[cellRow, cellColumn];
			if (targetCell is CellForUser cellForUser)
			{
				cellForUser.SetEmpty();
			}
			else
			{
				Debug.LogWarning("CAN'T CLEAN NUMBER ON GENERATED CELL");
			}
		}

		public bool CanPlaceValue(int valueToPlace, ICell cellToPlace)
		{
			HashSet<(int, int)> rows = new();
			HashSet<(int, int)> columns = new();
			HashSet<(int, int, int)> subGrids = new();
			foreach (ICell cell in CellsArray)
			{
				if (cell.IsEmpty) continue;
				if (cell.Row == cellToPlace.Row && cell.Column == cellToPlace.Column) continue;

				if (!rows.Add((cell.Row, cell.Number)))
				{
					return false;
				}

				if (!columns.Add((cell.Column, cell.Number)))
				{
					return false;
				}

				if (!subGrids.Add((cell.Row / _sudokuGridConfig.SubGridRows, cell.Column / _sudokuGridConfig.SubGridColumns, cell.Number)))
				{
					return false;
				}
			}

			if (!rows.Add((cellToPlace.Row, valueToPlace)))
			{
				return false;
			}

			if (!columns.Add((cellToPlace.Column, valueToPlace)))
			{
				return false;
			}

			if (!subGrids.Add((cellToPlace.Row / _sudokuGridConfig.SubGridRows, cellToPlace.Column / _sudokuGridConfig.SubGridColumns, valueToPlace)))
			{
				return false;
			}

			return true;
		}

		public void SetCellAsEmpty(ICell cell)
		{
			ICell temp = CellsArray[cell.Row, cell.Column];
			CellsArray[cell.Row, cell.Column] = new CellForUser(temp.Index, temp.GroupBox, temp.Row, temp.Column, 0, temp.Number);
		}

		public bool IsAllCellsFilledGood()
		{
			int count = CellsArray.Cast<ICell>().Count(cell => cell.IsFilledGood);
			return count == CellsArray.Length;
		}

		public ICell GetCell(int selectedCellIndex)
		{
			foreach (ICell cell in CellsArray)
			{
				if (cell.Index == selectedCellIndex)
					return cell;
			}
			return null;
		}
	}
}