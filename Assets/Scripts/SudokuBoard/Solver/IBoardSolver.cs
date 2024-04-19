using System;

namespace SudokuBoard.Solver
{
	public interface IBoardSolver
	{
		public bool Solve(ICell[,] cells, Func<int, ICell, bool> canPlaceValue, Func<bool> isBoardFullFilled);
	}
}