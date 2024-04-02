using System;
using System.Collections.Generic;
using Board;

namespace Gui.Gameplay.Models
{
	public class GameplayPanelModel
	{
		public event Action Setup;
		public event Action Refresh;
		public event Action PlaceNewNumber;

		public readonly SudokuBoard SudokuBoard;

		public ICell SelectedCell { get; private set; }

		public IEnumerable<int> Numbers => _playerNumberPlacement.AvailableNumbers;

		private readonly PlayerNumberPlacement _playerNumberPlacement;

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

			_playerNumberPlacement = new PlayerNumberPlacement(9);
		}

		public void Show()
		{
			Setup?.Invoke();
		}

		public void SelectCell(ICell selectedCell)
		{
			// _cellDisplays = _sudokuService.GetCellDisplays(selectedCellIndex);
			SelectedCell = selectedCell;

			Refresh?.Invoke();
		}

		public void PlaceNumber(int number)
		{
			SudokuBoard.PlaceValue(number, SelectedCell);
			SelectedCell = SudokuBoard.CellsArray[SelectedCell.Row, SelectedCell.Column]; 
			// _sudokuService.PlaceNumber(number, _selectedCellIndex);
			// _cellDisplays = _sudokuService.GetCellDisplays(_selectedCellIndex);
			PlaceNewNumber?.Invoke();
		}
	}
}