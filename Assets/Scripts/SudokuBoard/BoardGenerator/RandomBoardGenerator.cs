using System;
using Configs;
using SudokuBoard.Board;
using SudokuBoard.Solver;

namespace SudokuBoard.BoardGenerator
{
	public class RandomBoardGenerator : IBoardGenerator
	{
		private readonly SudokuGridConfig _sudokuGridConfig;
		private readonly IBoardSolver _boardSolver;
		private readonly Func<int, ICell, bool> _canPlaceValue;
		private readonly Func<bool> _isFullFilled;

		public RandomBoardGenerator(SudokuGridConfig sudokuGridConfig, IBoardSolver boardSolver, Func<int, ICell, bool> canPlaceValue, Func<bool> isFullFilled)
		{
			_sudokuGridConfig = sudokuGridConfig;
			_boardSolver = boardSolver;
			_canPlaceValue = canPlaceValue;
			_isFullFilled = isFullFilled;
		}

		public void Generate(ICell[,] cells)
		{
			int rows = _sudokuGridConfig.Rows;
			int subgridRows = _sudokuGridConfig.SubGridRows;
			int subgridColumns = _sudokuGridConfig.SubGridColumns;

			for (int row = 0; row < rows; row++)
			{
				for (int column = 0; column < rows; column++)
				{
					int groupBox = (row / subgridRows) + subgridColumns * (column / subgridColumns) + 1;
					cells[row, column] = new CellForUser(row * rows + column, groupBox, row, column, 0, 0);
				}
			}

			_boardSolver.Solve(cells, _canPlaceValue, _isFullFilled);
		}
	}
}