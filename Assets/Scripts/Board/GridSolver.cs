using System;
using System.Collections.Generic;
using NumberGenerator;

namespace Board
{
	public class GridSolver
	{
		private readonly IEnumerable<int> _numberList;

		public GridSolver()
		{
			RandomNumberListGenerator numberListGenerator = new RandomNumberListGenerator();
			_numberList = numberListGenerator.GenerateNumbers(9);
		}

		public bool Solve(int rows, int columns, ICell[,] cells, Func<int, ICell, bool> canPlaceValue, Func<bool> isBoardFullFilled)
		{
			for (int row = 0; row < rows; row++)
			{
				for (int column = 0; column < columns; column++)
				{
					ICell cell = cells[row, column];
					if (cell.IsEmpty)
					{
						foreach (int number in _numberList)
						{
							if (canPlaceValue.Invoke(number, cell))
							{
								int groupBox = (row / 3) + 3 * (column / 3) + 1;

								cells[row, column] = new SolvedByGeneratorCell(row * rows + column, groupBox, row, column, number);

								if (isBoardFullFilled.Invoke())
								{
									return true;
								}

								if (Solve(rows, columns, cells, canPlaceValue, isBoardFullFilled))
								{
									return true;
								}

								cells[row, column] = new CellForUser(row * rows + column, groupBox, row, column, 0, 0);
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