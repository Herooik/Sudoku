using System;
using System.Collections.Generic;
using Configs;
using SudokuBoard.NumberGenerator;

namespace SudokuBoard.Solver
{
	public class ExistedBoardSolver : IBoardSolver
	{
		private readonly SudokuGridConfig _sudokuGridConfig;
		private readonly IEnumerable<int> _numberList;

		public ExistedBoardSolver(SudokuGridConfig sudokuGridConfig)
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
				for (int col = 0; col < rows; col++)
				{
					ICell cell = board.GetCell(row, col);
					if (cell.IsEmpty)
					{
						foreach (int number in _numberList)
						{
							if (canPlaceValue.Invoke(row, col, number))
							{
								int groupBox = BoardHelper.GetGroupBoxNumber(row, col, subgridRows, subgridColumns);
								int index = BoardHelper.CalculateIndex(row, rows, col);

								board.SetCellAsUser(index, groupBox, row, col, number, true);

								if (isBoardFullFilled.Invoke())
								{
									return true;
								}

								if (Solve(board, canPlaceValue, isBoardFullFilled))
								{
									return true;
								}

								board.SetCellAsEmpty(index, groupBox, row, col);
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