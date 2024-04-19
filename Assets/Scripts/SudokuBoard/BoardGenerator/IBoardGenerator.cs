namespace SudokuBoard.BoardGenerator
{
	public interface IBoardGenerator
	{
		public void Generate(ICell[,] cells);
	}
}