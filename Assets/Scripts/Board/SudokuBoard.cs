using System;
using System.Collections.Generic;
using System.Linq;

namespace Board
{
	public class SudokuBoard
	{
		public IReadOnlyList<Cell> Cells => _cells;

		private readonly List<Cell> _cells = new();

		// todo: make support for other grid types 6x6, 8x8 etc.
		public void InitializeCells()
		{
			_cells.Clear();

			int rows = 9;
			int columns = 9;

			for (int row = 0; row < rows; row++)
			{
				for (int column = 0; column < columns; column++)
				{
					_cells.Add(
						new Cell(
							row * rows + column,
							row,
							column,
							(row / 3) + 3 * (column / 3) + 1,
							-1));
				}
			}
		}

		public bool IsFullFilled()
		{
			return _cells.All(cell => !cell.IsEmpty());
		}

		public void SetCellValue(int index, int value)
		{
			_cells[index].SetValue(value);
		}

		public bool IsValidValueForTheCell(int val, Cell cell)
		{
			if (_cells.Where(c => c.Index != cell.Index && c.GroupBox == cell.GroupBox).FirstOrDefault(c2 => c2.Value == val) != null)
				return false;

			if (IsValueInRow(val, cell))
				return false;

			if (IsValueInColumn(val, cell))
				return false;

			return true;
		}

		private bool IsValueInRow(int val, Cell cell)
		{
			foreach (Cell c in GetCellsInRow(cell.CellPosition.Row))
			{
				if (c.Value == val)
				{
					return true;
				}
			}
			return false;
		}

		private bool IsValueInColumn(int val, Cell cell)
		{
			foreach (Cell c in GetCellsInColumn(cell.CellPosition.Column))
			{
				if (c.Value == val)
				{
					return true;
				}
			}
			return false;
		}

		private IEnumerable<Cell> GetCellsInRow(int row)
		{
			if (row is < 0 or >= 9)
			{
				throw new ArgumentException("Row number must be between 0 and 8.");
			}

			return _cells.Where(cell => cell.CellPosition.Row == row).ToList().AsReadOnly();
		}

		private IEnumerable<Cell> GetCellsInColumn(int column)
		{
			if (column is < 0 or >= 9)
			{
				throw new ArgumentException("Row number must be between 0 and 8.");
			}

			return _cells.Where(cell => cell.CellPosition.Column == column).ToList().AsReadOnly();
		}
	}
}
