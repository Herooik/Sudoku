using System.Collections.Generic;
using Board;
using NumberGenerator;

namespace GridSolver
{
	public class GridSolver
	{
		private readonly SudokuBoard _sudokuBoard;
		private readonly IEnumerable<int> _numberList;

		public GridSolver(SudokuBoard sudokuBoard)
		{
			_sudokuBoard = sudokuBoard;

			RandomNumberListGenerator numberListGenerator = new RandomNumberListGenerator();
			_numberList = numberListGenerator.GenerateNumbers(9);
		}

		public bool Solve(SudokuBoard sudokuBoard)
		{
			foreach (Cell cell in sudokuBoard.Cells)
			{
				if (!cell.IsEmpty())
					continue;

				foreach (int number in _numberList)
				{
					if (_sudokuBoard.IsValidValueForTheCell(number, cell))
					{
						sudokuBoard.SetCellValue(cell.Index, number);

						if (_sudokuBoard.IsFullFilled())
						{
							return true;
						}

						if (Solve(sudokuBoard))
						{
							return true;
						}

						sudokuBoard.SetCellValue(cell.Index, -1);
					}
				}

				return false;
			}

			return true;
		}
	}
}