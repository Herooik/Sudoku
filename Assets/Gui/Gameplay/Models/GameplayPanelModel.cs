using System;
using System.Collections.Generic;
using Board;

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

		public GameplayPanelModel(SudokuType sudokuType = SudokuType.NINE_BY_NINE)
		{
			DisplayGridConfig rules = SudokuGridRules.GetRules(sudokuType);
			GridSolver gridSolver = new GridSolver();
			SudokuBoard = new SudokuBoard(rules.Rows, rules.Columns, gridSolver);
			SudokuBoard.GenerateNewBoard();

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