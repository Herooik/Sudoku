using Board;

public class SudokuService : ISudokuService
{
	private readonly SudokuBoard _sudokuBoard = new();

	public SudokuBoard CreateSolvableBoard(SudokuType sudokuType)
	{
		DisplayGridConfig rules = SudokuGridRules.GetRules(sudokuType);
		_sudokuBoard.InitializeCells(rules.Rows, rules.Columns);
		GridSolver gridSolver = new(_sudokuBoard);
		gridSolver.Solve(_sudokuBoard);
		RemoveRandomCellsHandler.RemoveRandomCellsFromBoard(_sudokuBoard.Cells, 10);
		return _sudokuBoard;
	}
}

public interface ISudokuService
{
	public SudokuBoard CreateSolvableBoard(SudokuType sudokuType);
}
