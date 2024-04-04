using System.Collections.Generic;
using BoardGenerator;
using UnityEngine;

namespace Board
{
	public class SudokuBoard
	{
		public readonly ICell[,] CellsArray;

		private readonly int _rows;
		private readonly int _columns;
		private readonly GridSolver _gridSolver;

		public SudokuBoard(int rows, int columns, GridSolver gridSolver)
		{
			_rows = rows;
			_columns = columns;
			_gridSolver = gridSolver;

			CellsArray = new ICell[rows, columns];
		}

		public void GenerateNewBoard(int cellsToRemove)
		{
			IBoardGenerator boardGenerator = new RandomBoardGenerator(_rows, _columns, _gridSolver, CanPlaceValue, IsFullFilled);
			boardGenerator.Generate(CellsArray);

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

		public int GetRowsLength()
		{
			return CellsArray.GetLength(0);
		}
		
		public int GetColumnsLength()
		{
			return CellsArray.GetLength(1);
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
			return count == GetColumnsLength();
		}

		public void PlaceValue(int value, ICell targetCell)
		{
			if (targetCell is CellForUser cellForUser)
			{
				if (cellForUser.Number == value)
				{
					cellForUser.SetEmpty();
				}
				else
				{
					cellForUser.FillCell(value);
				}
			}
			else
			{
				Debug.LogWarning("CAN'T PLACE NUMBER ON GENERATED CELL");
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

				if (!subGrids.Add((cell.Row / 3, cell.Column / 3, cell.Number)))
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

			if (!subGrids.Add((cellToPlace.Row / 3, cellToPlace.Column / 3, valueToPlace)))
			{
				return false;
			}

			return true;
		}

		public IEnumerable<ICell> GetFilledCellsWithSameNumber(int number)
		{
			List<ICell> cells = new();
			foreach (ICell cell in CellsArray)
			{
				if (!cell.IsEmpty && cell.Number == number)
						cells.Add(cell);
			}
			return cells;
		}
		
		public IEnumerable<ICell> GetCellsWithSameGroupBox(int groupBox)
		{
			List<ICell> cells = new();
			foreach (ICell cell in CellsArray)
			{
				if (cell.GroupBox == groupBox)
					cells.Add(cell);
			}
			return cells;
		}

		public void SetCellAsEmpty(ICell cell)
		{
			ICell temp = CellsArray[cell.Row, cell.Column];
			CellsArray[cell.Row, cell.Column] = new CellForUser(temp.Index, temp.GroupBox, temp.Row, temp.Column, 0, temp.Number);
		}
	}
}