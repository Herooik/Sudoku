public static class GridExtension
{
	public static int GetRowsLength<T>(this T[,] cells) => cells.GetLength(0);
}