using System;
using System.Collections.Generic;
using Board;
using Cells;
using Configs;
using NumberGenerator;

namespace Solver
{
	public class BoardSolver : IBoardSolver
	{
		private readonly SudokuGridConfig _sudokuGridConfig;
		private readonly IEnumerable<int> _numberList;

		public BoardSolver(SudokuGridConfig sudokuGridConfig)
		{
			_sudokuGridConfig = sudokuGridConfig;

			RandomNumberListGenerator numberListGenerator = new RandomNumberListGenerator();
			_numberList = numberListGenerator.GenerateNumbers(_sudokuGridConfig.Rows);
		}

		public bool Solve(Board.Board board, Func<int, int, int, bool> canPlaceValue, Func<bool> isBoardFullFilled)
		{
			int rows = _sudokuGridConfig.Rows;
			int subgridRows = _sudokuGridConfig.SubGridRows;
			int subgridColumns = _sudokuGridConfig.SubGridColumns;

			for (int row = 0; row < rows; row++)
			{
				for (int column = 0; column < rows; column++)
				{
					ICell cell = board.GetCell(row, column);
					if (cell.IsEmpty)
					{
						foreach (int number in _numberList)
						{
							if (canPlaceValue.Invoke(row, column, number))
							{
								int groupBox = BoardHelper.GetGroupBoxNumber(row, column, subgridRows, subgridColumns);
								int index = BoardHelper.CalculateIndex(row, column, rows);

								board.SetCellAsSolver(index, groupBox, row, column, number);

								if (isBoardFullFilled.Invoke())
								{
									return true;
								}

								if (Solve(board, canPlaceValue, isBoardFullFilled))
								{
									return true;
								}

								board.SetCellAsEmpty(index, groupBox, row, column);
								// board.SetCellAsUser(index, groupBox, row, column, 0, 0);
							}
						}

						return false;
					}
				}
			}

			return true;
		}
	}
}