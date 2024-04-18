public static class GridExtension
{
	public static int GetRowsLength<T>(this T[,] cells)
	{
		return cells.GetLength(0);
	}

	//todo: consider remove this and base only on rows count because in sudoku there is always the same amount rows == columns
	public static int GetColumnsLength<T>(this T[,] cells)
	{
		return cells.GetLength(1);
	}
}