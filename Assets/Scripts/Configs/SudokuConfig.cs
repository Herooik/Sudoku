using System;

namespace Configs
{
	public enum SudokuDifficulty
	{
		EASY,
		NORMAL,
		HARD,
		EXPERT,
		MASTER,
	}

	public enum SudokuType
	{
		FOUR_BY_FOUR,
		SIX_BY_SIX,
		EIGHT_BY_EIGHT,
		NINE_BY_NINE,
		TEN_BY_TEN,
		TWELVE_BY_TWELVE,
		SIXTEEN_BY_SIXTEEN,
	}

	public readonly struct SudokuGridConfig
	{
		public readonly int Rows;
		public readonly int SubGridRows;
		public readonly int SubGridColumns;

		public SudokuGridConfig(int rows, int subGridRows, int subGridColumns)
		{
			Rows = rows;
			SubGridRows = subGridRows;
			SubGridColumns = subGridColumns;
		}
	}

	public static class SudokuConfig
	{
		private static readonly SudokuGridConfig FourByFour = new SudokuGridConfig(4, 2, 2);
		private static readonly SudokuGridConfig SixBySix = new SudokuGridConfig(6, 2, 3);
		private static readonly SudokuGridConfig EightByEight = new SudokuGridConfig(8, 2, 4);
		private static readonly SudokuGridConfig NineByNine = new SudokuGridConfig(9, 3, 3);
		private static readonly SudokuGridConfig TenByTen = new SudokuGridConfig(10, 2, 5);
		private static readonly SudokuGridConfig TwelveByTwelve = new SudokuGridConfig(12, 3, 4);
		private static readonly SudokuGridConfig SixteenBySixteen = new SudokuGridConfig(16, 4, 4);

		public static SudokuGridConfig GetConfig(SudokuType sudokuType)
		{
			return sudokuType switch
			{
				SudokuType.FOUR_BY_FOUR => FourByFour,
				SudokuType.SIX_BY_SIX => SixBySix,
				SudokuType.EIGHT_BY_EIGHT => EightByEight,
				SudokuType.NINE_BY_NINE => NineByNine,
				SudokuType.TEN_BY_TEN => TenByTen,
				SudokuType.TWELVE_BY_TWELVE => TwelveByTwelve,
				SudokuType.SIXTEEN_BY_SIXTEEN => SixteenBySixteen,
				_ => throw new ArgumentOutOfRangeException(nameof(sudokuType), sudokuType, null)
			};
		}
	}
}