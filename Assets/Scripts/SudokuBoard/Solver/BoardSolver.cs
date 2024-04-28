using System;
using System.Collections.Generic;
using Configs;
using SudokuBoard.NumberGenerator;

namespace SudokuBoard.Solver
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

		public bool Solve(ICell[,] cells, Func<int, ICell, bool> canPlaceValue, Func<bool> isBoardFullFilled)
		{
			int rows = _sudokuGridConfig.Rows;
			int subgridRows = _sudokuGridConfig.SubGridRows;
			int subgridColumns = _sudokuGridConfig.SubGridColumns;

			for (int row = 0; row < rows; row++)
			{
				for (int column = 0; column < rows; column++)
				{
					ICell cell = cells[row, column];
					if (cell.IsEmpty)
					{
						foreach (int number in _numberList)
						{
							if (canPlaceValue.Invoke(number, cell))
							{
								int groupBox = BoardHelper.GetGroupBoxNumber(row, column, subgridRows, subgridColumns);
								int index = BoardHelper.CalculateIndex(row, rows, column);

								cells[row, column] = new SolverCell(index, groupBox, row, column, number);

								if (isBoardFullFilled.Invoke())
								{
									return true;
								}

								if (Solve(cells, canPlaceValue, isBoardFullFilled))
								{
									return true;
								}

								cells[row, column] = new UserCell(index, groupBox, row, column, 0, 0);
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