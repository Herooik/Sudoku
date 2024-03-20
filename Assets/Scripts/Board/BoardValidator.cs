using System.Collections.Generic;
using Board;

namespace BoardValidator
{
	public class BoardValidator
	{
		public bool ValidateGrid(IEnumerable<Cell> cells)
		{
			HashSet<(int, int)> rows = new();
			HashSet<(int, int)> columns = new();
			HashSet<(int, int, int)> subGrids = new();
			foreach (Cell cell in cells)
			{
				if (cell.IsEmpty())
					return false;

				int row = cell.CellPosition.Row;
				int col = cell.CellPosition.Column;
				int value = cell.ActualValue;

				if (!cell.IsEmpty())
				{
					if (!rows.Add((row, value)) ||
					    !columns.Add((col, value)) ||
					    !subGrids.Add((row / 3, col / 3, value)))
						return false;
				}
			}

			return true;
		}
	}
}