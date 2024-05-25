using System;
using Configs;

[Serializable]
public class SelectedGameSettings
{
	public SudokuType SudokuType = SudokuType.EIGHT_BY_EIGHT;
	public SudokuDifficulty Difficulty = SudokuDifficulty.NORMAL;
}