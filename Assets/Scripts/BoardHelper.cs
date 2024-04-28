public static class BoardHelper
{
	public static int GetGroupBoxNumber(int row, int column, int subgridRows, int subgridColumns)
	{
		int groupBox = (row / subgridRows) + subgridColumns * (column / subgridColumns);
		return groupBox;
	}

	public static int CalculateIndex(int row, int rows, int column)
	{
		return row * rows + column;
	}
}