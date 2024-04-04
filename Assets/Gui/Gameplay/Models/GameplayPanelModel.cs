using System;
using System.Collections.Generic;
using Board;
using Gui.ScriptableObjects;

namespace Gui.Gameplay.Models
{
	public class GameplayPanelModel
	{
		public event Action Refresh;

		public readonly SudokuBoard SudokuBoard;

		public ICell SelectedCell { get; private set; }

		public IReadOnlyList<int> AllNumbers => _inputNumbers.AllNumbers;
		public IEnumerable<int> AvailableNumbers => _inputNumbers.AvailableNumbers;

		private readonly InputNumbers _inputNumbers;

		public GameplayPanelModel(
			DifficultyRulesSettings difficultyRulesSettings,
			SudokuType sudokuType,
			SudokuDifficulty selectedDifficulty)
		{
			DisplayGridConfig rules = SudokuGridRules.GetRules(sudokuType);
			GridSolver gridSolver = new GridSolver();
			SudokuBoard = new SudokuBoard(rules.Rows, rules.Columns, gridSolver);
			SudokuBoard.GenerateNewBoard(difficultyRulesSettings.GetCellsToRemove(sudokuType, selectedDifficulty));

			Random random = new();
			int row = random.Next(0, SudokuBoard.GetRowsLength());
			int column = random.Next(0, SudokuBoard.GetColumnsLength());
			SelectedCell = SudokuBoard.CellsArray[row, column];

			_inputNumbers = new InputNumbers(9);
			RefreshAvailableInputNumbers();
		}

		public void SelectCell(ICell selectedCell)
		{
			SelectedCell = selectedCell;

			Refresh?.Invoke();
		}

		public void PlaceNumber(int number)
		{
			SudokuBoard.PlaceValue(number, SelectedCell);

			SelectedCell = SudokuBoard.CellsArray[SelectedCell.Row, SelectedCell.Column];

			RefreshAvailableInputNumbers();

			Refresh?.Invoke();
		}

		private void RefreshAvailableInputNumbers()
		{
			for (int i = 1; i <= SudokuBoard.GetRowsLength(); i++)
			{
				if (SudokuBoard.IsValueReachMaxOutUsed(i))
				{
					_inputNumbers.RemoveNumber(i);
				}
			}
		}
	}
}