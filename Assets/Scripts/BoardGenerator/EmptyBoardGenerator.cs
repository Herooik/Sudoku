namespace BoardGenerator
{
	public class EmptyBoardGenerator : IBoardGenerator
	{
		private readonly int _rows;
		private readonly int _columns;

		public EmptyBoardGenerator(int rows, int columns)
		{
			_rows = rows;
			_columns = columns;
		}

		public void Generate(ICell[,] cells)
		{
			for (int row = 0; row < _rows; row++)
			{
				for (int column = 0; column < _columns; column++)
				{
					int groupBox = (row / 3) + 3 * (column / 3) + 1;
					cells[row, column] = new CellForUser(row * _rows + column, groupBox, row, column, 0, 0);
				}
			}
		}
	}
}