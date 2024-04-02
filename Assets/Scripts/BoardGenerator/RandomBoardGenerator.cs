using System;
using Board;

namespace BoardGenerator
{
	public class RandomBoardGenerator : IBoardGenerator
	{
		private readonly int _rows;
		private readonly int _columns;
		private readonly GridSolver _gridSolver;
		private readonly Func<int, ICell, bool> _canPlaceValue;
		private readonly Func<bool> _isFullFilled;

		public RandomBoardGenerator(int rows, int columns, GridSolver gridSolver, Func<int, ICell, bool> canPlaceValue, Func<bool> isFullFilled)
		{
			_rows = rows;
			_columns = columns;
			_gridSolver = gridSolver;
			_canPlaceValue = canPlaceValue;
			_isFullFilled = isFullFilled;
		}

		public void Generate(ICell[,] cells)
		{
			for (int row = 0; row < _rows; row++)
			{
				for (int column = 0; column < _columns; column++)
				{
					int groupBox = (row / 3) + 3 * (column / 3) + 1;
					cells[row, column] = new EmptyCell(row * _rows + column, groupBox, row, column);
				}
			}

			_gridSolver.Solve(_rows, _columns, cells, _canPlaceValue, _isFullFilled);
		}
	}
}