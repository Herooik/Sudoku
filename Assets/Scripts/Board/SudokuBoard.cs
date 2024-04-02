using System.Collections.Generic;
using BoardGenerator;

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

		public void GenerateNewBoard()
		{
			IBoardGenerator boardGenerator = new RandomBoardGenerator(_rows, _columns, _gridSolver, CanPlaceValue, IsFullFilled);
			boardGenerator.Generate(CellsArray);

			RemoveRandomCellsHandler.RemoveRandomCellsFromBoard(CellsArray, 20);
		}

		public bool IsFullFilled()
		{
			foreach (ICell cell in CellsArray)
			{
				if (cell is ICellNumber cellNumber)
				{
					if (cellNumber.Number <= 0 )
						return false;
				}
				// if (cell is EmptyCell)
					// return false;
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
				if (cell is ICellNumber iCellNumber)
				{
					if (iCellNumber.Number == value)
						count++;
				}
			}
			// int count = _cells.Count(cell => cell.ActualValue == value);
			return count == GetColumnsLength();
		}

		public void PlaceValue(int value, ICell targetCell)
		{
			// bool isPlacedGood = CanPlaceValue(value, targetCell);

			// CellsArray[targetCell.Row, targetCell.Column] = new CellFilledByUserInput(targetCell.Index,
			// 	targetCell.GroupBox,
			// 	targetCell.Row,
			// 	targetCell.Column,
			// 	value,
			// 	isPlacedGood);
		}

		public enum PlaceValueResult
		{
			OK,
			FILLED_BY_SOLVER,
			DUPLICATE_IN_SUB_BOX,
			DUPLICATE_IN_ROW,
			DUPLICATE_IN_COLUMN,
		}

		public bool CanPlaceValue(int valueToPlace, ICell cellToPlace)
		{
			HashSet<(int, int)> rows = new();
			HashSet<(int, int)> columns = new();
			HashSet<(int, int, int)> subGrids = new();
			foreach (ICell cell in CellsArray)
			{
				if (cell is ICellNumber cellNumber)
				{
					// Skip the cell we're checking
					if (cell.Row == cellToPlace.Row && cell.Column == cellToPlace.Column) continue;

					if (!rows.Add((cell.Row, cellNumber.Number)))
					{
						return false;
					}

					if (!columns.Add((cell.Column, cellNumber.Number)))
					{
						return false;
					}

					if (!subGrids.Add((cell.Row / 3, cell.Column / 3, cellNumber.Number)))
					{
						return false;
					}
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

		public IEnumerable<ICell> GetCellsWithSameNumber(int number)
		{
			List<ICell> cells = new();
			foreach (ICell cell in CellsArray)
			{
				if (cell is ICellNumber cellNumber)
				{
					if (cellNumber.Number == number)
						cells.Add(cell);
				}
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
	}
}