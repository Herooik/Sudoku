using System;

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

public struct DisplayGridConfig
{
	public int Columns;
	public int Rows;
	public int GroupBoxColumns;
	public int GroupBoxRows;
}

public static class SudokuGridRules
{
	public static DisplayGridConfig GetRules(SudokuType sudokuType)
	{
		switch (sudokuType)
		{
			case SudokuType.FOUR_BY_FOUR:
				return FOUR_BY_FOUR;
			case SudokuType.SIX_BY_SIX:
				return SIX_BY_SIX;
			case SudokuType.EIGHT_BY_EIGHT:
				return EIGHT_BY_EIGHT;
			case SudokuType.NINE_BY_NINE:
				return NINE_BY_NINE;
			case SudokuType.TEN_BY_TEN:
			case SudokuType.TWELVE_BY_TWELVE:
			case SudokuType.SIXTEEN_BY_SIXTEEN:
				break;
		}

		throw new ArgumentOutOfRangeException(nameof(sudokuType), sudokuType, null);
	}

	public static DisplayGridConfig FOUR_BY_FOUR = new DisplayGridConfig()
	{
		Columns = 4,
		Rows = 4,
		GroupBoxColumns = 2,
		GroupBoxRows = 2,
	};

	public static DisplayGridConfig SIX_BY_SIX = new DisplayGridConfig()
	{
		Columns = 6,
		Rows = 6,
		GroupBoxColumns = 2,
		GroupBoxRows = 3,
	};

	public static DisplayGridConfig EIGHT_BY_EIGHT = new DisplayGridConfig()
	{
		Columns = 8,
		Rows = 8,
		GroupBoxColumns = 2,
		GroupBoxRows = 4,
	};

	public static DisplayGridConfig NINE_BY_NINE = new DisplayGridConfig()
	{
		Columns = 9,
		Rows = 9,
		GroupBoxColumns = 3,
		GroupBoxRows = 3,
	};
}