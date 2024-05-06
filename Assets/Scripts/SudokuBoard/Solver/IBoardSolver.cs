using System;

namespace SudokuBoard.Solver
{
	public interface IBoardSolver
	{
		public bool Solve(Board.Board board, Func<int, ICell, bool> canPlaceValue, Func<bool> isBoardFullFilled);
	}
}