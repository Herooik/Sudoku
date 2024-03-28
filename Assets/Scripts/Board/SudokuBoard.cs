using System.Collections.Generic;
using System.Linq;

namespace Board
{
	public class SudokuBoard
	{
		public int Columns { get; private set; }
		public IReadOnlyList<Cell> Cells => _cells;

		private readonly List<Cell> _cells = new();

		public Dictionary<int, List<Cell>> _rows = new Dictionary<int, List<Cell>>();
		public Dictionary<int, List<Cell>> _columns = new Dictionary<int, List<Cell>>();
		public Dictionary<int, List<Cell>> _groupBoxes = new Dictionary<int, List<Cell>>();

		// todo: make support for other grid types 6x6, 8x8 etc.
		public void InitializeCells(int rows, int columns)
		{
			Columns = columns;
			_cells.Clear();

			for (int row = 0; row < rows; row++)
			{
				_rows[row] = new List<Cell>();
				for (int column = 0; column < columns; column++)
				{
					int groupBox = (row / 3) + 3 * (column / 3) + 1;
					Cell cell = new Cell(
						row * rows + column,
						row,
						column,
						groupBox,
						-1);
					_cells.Add(cell);
					
					_rows[row].Add(cell);

					if (!_columns.ContainsKey(column))
					{
						_columns[column] = new List<Cell>();
					}
					_columns[column].Add(cell);
					
					if (!_groupBoxes.ContainsKey(groupBox))
					{
						_groupBoxes[groupBox] = new List<Cell>();
					}
					_groupBoxes[groupBox].Add(cell);
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

		public enum PlaceValueResult
		{
			OK,
			FILLED_BY_SOLVER,
			DUPLICATE_IN_SUB_BOX,
			DUPLICATE_IN_ROW,
			DUPLICATE_IN_COLUMN,
		}

		public PlaceValueResult CanPlaceValue(int valueToPlace, Cell cellToPlace)
		{
			HashSet<(int, int)> rows = new();
			HashSet<(int, int)> columns = new();
			HashSet<(int, int, int)> subGrids = new();
			foreach (Cell cell in _cells.Where(cell => !cell.IsEmpty()))
			{
				// Skip the cell we're checking
				if (cell.Row == cellToPlace.Row &&
				    cell.Column == cellToPlace.Column)
					continue;

				if (!rows.Add((cell.Row, cell.ActualValue)))
				{
					return PlaceValueResult.DUPLICATE_IN_ROW;
				}

				if (!columns.Add((cell.Column, cell.ActualValue)))
				{
					return PlaceValueResult.DUPLICATE_IN_COLUMN;
				}

				if (!subGrids.Add((cell.Row / 3, cell.Column / 3, cell.ActualValue)))
				{
					return PlaceValueResult.DUPLICATE_IN_SUB_BOX;
				}
			}

			if (!rows.Add((cellToPlace.Row, valueToPlace)))
			{
				return PlaceValueResult.DUPLICATE_IN_ROW;
			}

			if (!columns.Add((cellToPlace.Column, valueToPlace)))
			{
				return PlaceValueResult.DUPLICATE_IN_COLUMN;
			}

			if (!subGrids.Add((cellToPlace.Row / 3, cellToPlace.Column / 3, valueToPlace)))
			{
				return PlaceValueResult.DUPLICATE_IN_SUB_BOX;
			}

			return PlaceValueResult.OK;
		}
	}
}