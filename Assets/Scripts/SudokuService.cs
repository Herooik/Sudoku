using Board;

public class SudokuService : ISudokuService
{
	private readonly SudokuBoard _sudokuBoard = new();

	public SudokuBoard GetSolvableBoard()
	{
		_sudokuBoard.InitializeCells();
		GridSolver.GridSolver gridSolver = new(_sudokuBoard);
		gridSolver.Solve(_sudokuBoard);
		RemoveRandomCellsHandler.RemoveRandomCellsFromGrid(_sudokuBoard.Cells, 20);
		return _sudokuBoard;
	}
}

public interface ISudokuService
{
	public SudokuBoard GetSolvableBoard();
}
