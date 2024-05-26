using System;
using Board;
using Configs;
using Solver;

namespace BoardGenerator
{
	public class RandomBoardGenerator : IBoardGenerator
	{
		private readonly SudokuGridConfig _sudokuGridConfig;
		private readonly IBoardSolver _boardSolver;
		private readonly Func<int, int, int, bool> _canPlaceValue;
		private readonly Func<bool> _isFullFilled;

		public RandomBoardGenerator(SudokuGridConfig sudokuGridConfig, IBoardSolver boardSolver, Func<int, int, int, bool> canPlaceValue, Func<bool> isFullFilled)
		{
			_sudokuGridConfig = sudokuGridConfig;
			_boardSolver = boardSolver;
			_canPlaceValue = canPlaceValue;
			_isFullFilled = isFullFilled;
		}

		public void Generate(Board.Board board)
		{
			int rows = _sudokuGridConfig.Rows;
			int subgridRows = _sudokuGridConfig.SubGridRows;
			int subgridColumns = _sudokuGridConfig.SubGridColumns;

			for (int row = 0; row < rows; row++)
			{
				for (int column = 0; column < rows; column++)
				{
					int groupBox = BoardHelper.GetGroupBoxNumber(row, column, subgridRows, subgridColumns);
					int index = BoardHelper.CalculateIndex(row, column, rows);
					board.SetCellAsEmpty(index, groupBox, row, column);
				}
			}

			_boardSolver.Solve(board, _canPlaceValue, _isFullFilled);
		}
	}
}