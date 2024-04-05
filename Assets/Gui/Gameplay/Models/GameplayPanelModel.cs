using System;
using System.Collections.Generic;
using Board;
using Gui.ScriptableObjects;
using Root;

namespace Gui.Gameplay.Models
{
	public class GameplayPanelModel
	{
		private readonly ApplicationNavigation _applicationNavigation;
		public event Action Refresh;

		public readonly SudokuBoard SudokuBoard;

		public ICell SelectedCell { get; private set; }
		public IReadOnlyList<int> AllNumbers => _inputNumbers.AllNumbers;
		public IEnumerable<int> AvailableNumbers => _inputNumbers.AvailableNumbers;
		public bool AutoSolverActive { get; private set; }

		private readonly InputNumbers _inputNumbers;

		public GameplayPanelModel(
			ApplicationNavigation applicationNavigation,
			DifficultyRulesSettings difficultyRulesSettings,
			SudokuType sudokuType,
			SudokuDifficulty selectedDifficulty)
		{
			_applicationNavigation = applicationNavigation;

			DisplayGridConfig rules = SudokuGridRules.GetRules(sudokuType);
			IBoardSolver boardSolver = new BoardSolver();
			SudokuBoard = new SudokuBoard(rules.Rows, rules.Columns, boardSolver);
			SudokuBoard.GenerateNewBoard(difficultyRulesSettings.GetCellsToRemove(sudokuType, selectedDifficulty));

			Random random = new();
			int row = random.Next(0, SudokuBoard.CellsArray.GetRowsLength());
			int column = random.Next(0, SudokuBoard.CellsArray.GetColumnsLength());
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

			bool isGameEnd = SudokuBoard.IsGameEnd();
			if (isGameEnd)
			{
				_applicationNavigation.OpenMainMenu();
			}

			Refresh?.Invoke();
		}

		public void AutoSolveBoard()
		{
			IBoardSolver boardSolver = new ExistedBoardSolverTEMP();
			bool solved = boardSolver.Solve(SudokuBoard.CellsArray, SudokuBoard.CanPlaceValue,
				SudokuBoard.IsFullFilled);

			Refresh?.Invoke();
		}

		public void CleanCell()
		{
			SudokuBoard.PlaceValue(SelectedCell.Number, SelectedCell);

			SelectedCell = SudokuBoard.CellsArray[SelectedCell.Row, SelectedCell.Column];

			RefreshAvailableInputNumbers();

			Refresh?.Invoke();
		}

		private void RefreshAvailableInputNumbers()
		{
			for (int i = 1; i <= SudokuBoard.CellsArray.GetRowsLength(); i++)
			{
				if (SudokuBoard.IsValueReachMaxOutUsed(i))
				{
					_inputNumbers.RemoveNumber(i);
				}
			}
		}
	}
}