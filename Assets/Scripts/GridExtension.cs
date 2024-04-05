public static class GridExtension
{
	public static int GetRowsLength<T>(this T[,] cells)
	{
		return cells.GetLength(0);
	}

	public static int GetColumnsLength<T>(this T[,] cells)
	{
		return cells.GetLength(1);
	}
}