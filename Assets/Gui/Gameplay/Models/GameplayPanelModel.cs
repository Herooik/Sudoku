using System;
using System.Collections.Generic;
using Board;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gui.Gameplay.Models
{
	public class GameplayPanelModel
	{
		public event Action Setup;
		public event Action Refresh;

		public IEnumerable<Cell> Cells => _sudokuBoard.Cells;
		public Cell SelectedCell { get; private set; }
		public IEnumerable<int> Numbers => _playerNumberPlacement.AvailableNumbers;

		private readonly SudokuService _sudokuService = new();
		private readonly SudokuBoard _sudokuBoard; //todo consider move to SudokuService
		private readonly PlayerNumberPlacement _playerNumberPlacement;

		public GameplayPanelModel()
		{
			_sudokuBoard = _sudokuService.CreateSolvableBoard(SudokuType.NINE_BY_NINE);
			_playerNumberPlacement = new PlayerNumberPlacement(_sudokuBoard.Columns);

			SelectedCell = _sudokuBoard.Cells[Random.Range(0, _sudokuBoard.Cells.Count)];
		}

		public void Show()
		{
			Setup?.Invoke();
		}

		public void SelectCell(Cell cell)
		{
			SelectedCell = cell;

			Refresh?.Invoke();
		}

		public void PlaceNumber(int number)
		{
			if (!SelectedCell.IsRemoved) return;

			if(SelectedCell.ActualValue == number)
			{
				SelectedCell.SetAsEmpty();
			}
			else
			{
				SelectedCell.SetValue(number);
			}

			SudokuBoard.Result isValidValueForTheCell = _sudokuBoard.CanPlaceValue(number, SelectedCell);
			Debug.LogWarning(isValidValueForTheCell);

			Refresh?.Invoke();
		}
	}
}