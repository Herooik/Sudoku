using System;
using System.Collections.Generic;
using NumberGenerator;

namespace Board
{
	public class BoardSolver : IBoardSolver
	{
		private readonly IEnumerable<int> _numberList;

		public BoardSolver()
		{
			RandomNumberListGenerator numberListGenerator = new RandomNumberListGenerator();
			_numberList = numberListGenerator.GenerateNumbers(9);
		}

		public bool Solve(ICell[,] cells, Func<int, ICell, bool> canPlaceValue, Func<bool> isBoardFullFilled)
		{
			int rows = cells.GetRowsLength();
			int columns = cells.GetColumnsLength();
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

								if (Solve(cells, canPlaceValue, isBoardFullFilled))
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

	public class ExistedBoardSolverTEMP : IBoardSolver
	{
		private readonly IEnumerable<int> _numberList;

		public ExistedBoardSolverTEMP()
		{
			RandomNumberListGenerator numberListGenerator = new RandomNumberListGenerator();
			_numberList = numberListGenerator.GenerateNumbers(9);
		}

		public bool Solve(ICell[,] cells, Func<int, ICell, bool> canPlaceValue, Func<bool> isBoardFullFilled)
		{
			int rows = cells.GetRowsLength();
			int columns = cells.GetColumnsLength();
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

								cells[row, column] = new CellForUser(row * rows + column, groupBox, row, column, number, number);

								if (isBoardFullFilled.Invoke())
								{
									return true;
								}

								if (Solve(cells, canPlaceValue, isBoardFullFilled))
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

	public interface IBoardSolver
	{
		public bool Solve(ICell[,] cells, Func<int, ICell, bool> canPlaceValue, Func<bool> isBoardFullFilled);
	}
}