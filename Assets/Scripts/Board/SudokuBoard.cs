using System.Collections.Generic;
using System.Linq;

namespace Board
{
	public class SudokuBoard
	{
		public int Columns { get; private set; }
		public IReadOnlyList<Cell> Cells => _cells;

		private readonly List<Cell> _cells = new();

		// todo: make support for other grid types 6x6, 8x8 etc.
		public void InitializeCells(int rows, int columns)
		{
			Columns = columns;
			_cells.Clear();

			for (int row = 0; row < rows; row++)
			{
				for (int column = 0; column < columns; column++)
				{
					int groupBox = (row / 3) + 3 * (column / 3) + 1;
					_cells.Add(new Cell(
						row * rows + column,
						row,
						column,
						groupBox,
						-1));
				}
			}
		}

		public bool IsFullFilled()
		{
			return _cells.All(cell => !cell.IsEmpty());
		}

		public bool IsValueReachMaxOutUsed(int value) //todo CHANGE NAME
		{
			int count = _cells.Count(cell => cell.ActualValue == value);
			return count == Columns;
		}

		public enum Result
		{
			OK,
			DUPLICATE_IN_SUB_BOX,
			DUPLICATE_IN_ROW,
			DUPLICATE_IN_COLUMN,
		}

		public Result CanPlaceValue(int valueToPlace, Cell cellToPlace)
		{
			HashSet<(int, int)> rows = new();
			HashSet<(int, int)> columns = new();
			HashSet<(int, int, int)> subGrids = new();
			foreach (Cell cell in _cells.Where(cell => !cell.IsEmpty()))
			{
				// Skip the cell we're checking
				if (cell.CellPosition.Row == cellToPlace.CellPosition.Row &&
				    cell.CellPosition.Column == cellToPlace.CellPosition.Column)
					continue;

				if (!rows.Add((cell.CellPosition.Row, cell.ActualValue)))
				{
					return Result.DUPLICATE_IN_ROW;
				}

				if (!columns.Add((cell.CellPosition.Column, cell.ActualValue)))
				{
					return Result.DUPLICATE_IN_COLUMN;
				}

				if (!subGrids.Add((cell.CellPosition.Row / 3, cell.CellPosition.Column / 3, cell.ActualValue)))
				{
					return Result.DUPLICATE_IN_SUB_BOX;
				}
			}

			if (!rows.Add((cellToPlace.CellPosition.Row, valueToPlace)))
			{
				return Result.DUPLICATE_IN_ROW;
			}

			if (!columns.Add((cellToPlace.CellPosition.Column, valueToPlace)))
			{
				return Result.DUPLICATE_IN_COLUMN;
			}

			if (!subGrids.Add((cellToPlace.CellPosition.Row / 3, cellToPlace.CellPosition.Column / 3, valueToPlace)))
			{
				return Result.DUPLICATE_IN_SUB_BOX;
			}

			return Result.OK;
		}
	}
}