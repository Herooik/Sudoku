using System;
using Board;

namespace BoardGenerator
{
	public class RandomBoardGenerator : IBoardGenerator
	{
		private readonly int _rows;
		private readonly int _columns;
		private readonly IBoardSolver _boardSolver;
		private readonly Func<int, ICell, bool> _canPlaceValue;
		private readonly Func<bool> _isFullFilled;

		public RandomBoardGenerator(int rows, int columns, IBoardSolver boardSolver, Func<int, ICell, bool> canPlaceValue, Func<bool> isFullFilled)
		{
			_rows = rows;
			_columns = columns;
			_boardSolver = boardSolver;
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
					cells[row, column] = new CellForUser(row * _rows + column, groupBox, row, column, 0, 0);
				}
			}

			_boardSolver.Solve(cells, _canPlaceValue, _isFullFilled);
		}
	}
}