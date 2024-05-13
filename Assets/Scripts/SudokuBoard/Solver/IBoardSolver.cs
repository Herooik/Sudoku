using System;

namespace SudokuBoard.Solver
{
	public interface IBoardSolver
	{
		public bool Solve(Board.Board board, Func<int, int, int, bool> canPlaceValue, Func<bool> isBoardFullFilled);
	}
}