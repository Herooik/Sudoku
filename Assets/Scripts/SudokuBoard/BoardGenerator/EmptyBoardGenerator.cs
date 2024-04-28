using Configs;

namespace SudokuBoard.BoardGenerator
{
	public class EmptyBoardGenerator : IBoardGenerator
	{
		private readonly SudokuGridConfig _sudokuGridConfig;

		public EmptyBoardGenerator(SudokuGridConfig sudokuGridConfig)
		{
			_sudokuGridConfig = sudokuGridConfig;
		}

		public void Generate(ICell[,] cells)
		{
			int rows = _sudokuGridConfig.Rows;
			int subgridRows = _sudokuGridConfig.SubGridRows;
			int subgridColumns = _sudokuGridConfig.SubGridColumns;

			for (int row = 0; row < rows; row++)
			{
				for (int column = 0; column < rows; column++)
				{
					int groupBox = BoardHelper.GetGroupBoxNumber(row, column, subgridRows, subgridColumns);
					int index = BoardHelper.CalculateIndex(row, rows, column);
					cells[row, column] = new UserCell(index, groupBox, row, column, 0, 0);
				}
			}
		}
	}
}