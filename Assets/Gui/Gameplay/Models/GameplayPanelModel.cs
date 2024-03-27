using System;
using System.Collections.Generic;

namespace Gui.Gameplay.Models
{
	public class GameplayPanelModel
	{
		public event Action Setup;
		public event Action Refresh;

		public IReadOnlyList<CellData> _cellDisplays;
		public IEnumerable<int> Numbers => _playerNumberPlacement.AvailableNumbers;

		private readonly ISudokuService _sudokuService = new SudokuService();
		private readonly PlayerNumberPlacement _playerNumberPlacement;

		private int _selectedCellIndex;

		public GameplayPanelModel()
		{
			_cellDisplays = _sudokuService.Initialize(SudokuType.NINE_BY_NINE);

			Random random = new();
			_selectedCellIndex = _cellDisplays[random.Next(0, _cellDisplays.Count)].Index;

			_playerNumberPlacement = new PlayerNumberPlacement(9);
		}

		public void Show()
		{
			Setup?.Invoke();
		}

		public void SelectCell(int selectedCellIndex)
		{
			_cellDisplays = _sudokuService.GetCellDisplays(selectedCellIndex);
			_selectedCellIndex = _cellDisplays[selectedCellIndex].Index;

			Refresh?.Invoke();
		}

		public void PlaceNumber(int number)
		{
			_sudokuService.PlaceNumber(number, _selectedCellIndex);
			_cellDisplays = _sudokuService.GetCellDisplays(_selectedCellIndex);
			Refresh?.Invoke();
		}
	}
}