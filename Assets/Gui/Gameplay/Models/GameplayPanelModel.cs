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
		public CellDisplayData[,] CellDisplayDatas;

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

			CellDisplayDatas = new CellDisplayData[rules.Rows, rules.Columns];
			RefreshCellDisplays();
		}

		public void SelectCell(ICell selectedCell)
		{
			SelectedCell = selectedCell;

			RefreshCellDisplays();
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

			RefreshCellDisplays();
			Refresh?.Invoke();
		}

		public void AutoSolveBoard()
		{
			IBoardSolver boardSolver = new ExistedBoardSolverTEMP();
			bool solved = boardSolver.Solve(SudokuBoard.CellsArray, SudokuBoard.CanPlaceValue,
				SudokuBoard.IsFullFilled);

			SelectedCell = SudokuBoard.CellsArray[SelectedCell.Row, SelectedCell.Column];

			RefreshCellDisplays();
			Refresh?.Invoke();
		}

		public void CleanCell()
		{
			SudokuBoard.PlaceValue(SelectedCell.Number, SelectedCell);

			SelectedCell = SudokuBoard.CellsArray[SelectedCell.Row, SelectedCell.Column];

			RefreshAvailableInputNumbers();

			RefreshCellDisplays();
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

		private void RefreshCellDisplays()
		{
			foreach (ICell cell in SudokuBoard.CellsArray)
			{
				CellDisplayDatas[cell.Row, cell.Column] = new CellDisplayData(cell.Row, cell.Column, State.DEFAULT, cell.IsFilledGood, cell.Number, cell.IsEmpty); 
			}

			IEnumerable<ICell> cellsWithSameNumber = SudokuBoard.GetFilledCellsWithSameNumber(SelectedCell.Number);
			foreach (ICell cell in cellsWithSameNumber)
			{
				CellDisplayDatas[cell.Row, cell.Column] = new CellDisplayData(cell.Row, cell.Column, State.SAME_NUMBER, cell.IsFilledGood, cell.Number, cell.IsEmpty); 
			}

			for (int i = 0; i < SudokuBoard.CellsArray.GetRowsLength(); i++)
			{
				ICell cell = SudokuBoard.CellsArray[i, SelectedCell.Column];
				if (cell.Number == SelectedCell.Number && !cell.IsEmpty)
				{
					CellDisplayDatas[i, SelectedCell.Column] = new CellDisplayData(cell.Row, cell.Column, State.SAME_WRONG_NUMBER_IN_ROW_COLUMN_GROUP, cell.IsFilledGood, cell.Number, cell.IsEmpty);
				}
				else
				{
					CellDisplayDatas[i, SelectedCell.Column] = new CellDisplayData(cell.Row, cell.Column, State.SAME_ROW_COLUMN_GROUP, cell.IsFilledGood, cell.Number, cell.IsEmpty);
				}
			}

			for (int i = 0; i < SudokuBoard.CellsArray.GetColumnsLength(); i++)
			{
				ICell cell = SudokuBoard.CellsArray[SelectedCell.Row, i];
				if (cell.Number == SelectedCell.Number && !cell.IsEmpty)
				{
					CellDisplayDatas[SelectedCell.Row, i] = new CellDisplayData(cell.Row, cell.Column, State.SAME_WRONG_NUMBER_IN_ROW_COLUMN_GROUP, cell.IsFilledGood, cell.Number, cell.IsEmpty);
				}
				else
				{
					CellDisplayDatas[SelectedCell.Row, i] = new CellDisplayData(cell.Row, cell.Column, State.SAME_ROW_COLUMN_GROUP, cell.IsFilledGood, cell.Number, cell.IsEmpty);
				}
			}

			foreach (ICell cell in SudokuBoard.GetCellsWithSameGroupBox(SelectedCell.GroupBox))
			{
				if (cell.Number == SelectedCell.Number && !cell.IsEmpty)
				{
					CellDisplayDatas[cell.Row, cell.Column] = new CellDisplayData(cell.Row, cell.Column, State.SAME_WRONG_NUMBER_IN_ROW_COLUMN_GROUP, cell.IsFilledGood, cell.Number, cell.IsEmpty);
				}
				else
				{
					CellDisplayDatas[cell.Row, cell.Column] = new CellDisplayData(cell.Row, cell.Column, State.SAME_ROW_COLUMN_GROUP, cell.IsFilledGood, cell.Number, cell.IsEmpty);
				}
			}

			CellDisplayDatas[SelectedCell.Row, SelectedCell.Column] = new CellDisplayData(SelectedCell.Row, SelectedCell.Column, State.SELECTED, SelectedCell.IsFilledGood, SelectedCell.Number, SelectedCell.IsEmpty);
		}
	}
}

public enum State
{
	DEFAULT,
	SELECTED,
	SAME_NUMBER,
	SAME_ROW_COLUMN_GROUP,
	SAME_WRONG_NUMBER_IN_ROW_COLUMN_GROUP
}

public readonly struct CellDisplayData
{
	public readonly int Row;
	public readonly int Column;
	public readonly State State;
	public readonly bool IsFilledGood;
	public string Num => _isEmpty ? string.Empty : _number.ToString();

	private readonly int _number;
	private readonly bool _isEmpty;

	public CellDisplayData(int row, int column, State state, bool isFilledGood, int number, bool isEmpty)
	{
		Row = row;
		Column = column;
		State = state;
		IsFilledGood = isFilledGood;

		_number = number;
		_isEmpty = isEmpty;
	}
}