using System;
using System.Collections.Generic;
using Board;
using Gui.ScriptableObjects;

namespace Gui.Gameplay.Models
{
	public class GameplayPanelModel
	{
		private readonly ApplicationNavigation _applicationNavigation;
		public event Action Refresh;

		public readonly SudokuBoard SudokuBoard;

		public IReadOnlyList<int> AllNumbers => _inputNumbers.AllNumbers;
		public IEnumerable<int> AvailableNumbers => _inputNumbers.AvailableNumbers;
		public IReadOnlyList<CellDisplayData> CellDisplayDataList => _cellDisplayDataList;

		private readonly InputNumbers _inputNumbers;
		private readonly List<CellDisplayData> _cellDisplayDataList;

		private ICell _selectedCell => SudokuBoard.GetCell(_selectedCellIndex);
		private int _selectedCellIndex;

		public GameplayPanelModel(
			ApplicationNavigation applicationNavigation,
			DifficultyRulesSettings difficultyRulesSettings,
			SelectedGameSettings selectedGameSettings)
		{
			_applicationNavigation = applicationNavigation;

			DisplayGridConfig rules = SudokuGridRules.GetRules(selectedGameSettings.SudokuType);
			IBoardSolver boardSolver = new BoardSolver();
			SudokuBoard = new SudokuBoard(rules.Rows, rules.Columns, boardSolver);
			SudokuBoard.GenerateNewBoard(difficultyRulesSettings.GetCellsToRemove(selectedGameSettings.SudokuType, selectedGameSettings.Difficulty));

			_cellDisplayDataList = new List<CellDisplayData>(new CellDisplayData[SudokuBoard.CellsArray.Length]);

			Random random = new();
			int row = random.Next(0, rules.Rows);
			int column = random.Next(0, rules.Columns);
			_selectedCellIndex = SudokuBoard.CellsArray[row, column].Index;

			_inputNumbers = new InputNumbers(9);

			RefreshAvailableInputNumbers();
			RefreshCellDisplays();
		}

		public void SelectCell(int selectedCellIndex)
		{
			_selectedCellIndex = selectedCellIndex;

			RefreshCellDisplays();
			Refresh?.Invoke();
		}

		public void PlaceNumber(int number)
		{
			if (_selectedCell.Number == number)
			{
				SudokuBoard.CleanCell(_selectedCell.Row, _selectedCell.Column);
			}
			else
			{
				SudokuBoard.PlaceValue(number, _selectedCell.Row, _selectedCell.Column);
			}

			bool isGameEnd = SudokuBoard.IsGameEnd();
			if (isGameEnd)
			{
				_applicationNavigation.OpenMainMenu();
			}

			RefreshAvailableInputNumbers();
			RefreshCellDisplays();
			Refresh?.Invoke();
		}

		public void AutoSolveBoard()
		{
			IBoardSolver boardSolver = new ExistedBoardSolverTEMP();
			boardSolver.Solve(SudokuBoard.CellsArray, SudokuBoard.CanPlaceValue, SudokuBoard.IsFullFilled);

			RefreshAvailableInputNumbers();
			RefreshCellDisplays();
			Refresh?.Invoke();
		}

		public void CleanCell()
		{
			SudokuBoard.CleanCell(_selectedCell.Row, _selectedCell.Column);

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
				else
				{
					_inputNumbers.AddNumber(i);
				}
			}
		}

		private void RefreshCellDisplays()
		{
			foreach (ICell cell in SudokuBoard.CellsArray)
			{
				State state = State.DEFAULT;

				if (!cell.IsEmpty && cell.Number == _selectedCell.Number)
				{
					state = State.SAME_NUMBER;
				}

				if (cell.Row == _selectedCell.Row || cell.Column == _selectedCell.Column || cell.GroupBox == _selectedCell.GroupBox)
				{
					state = State.SAME_ROW_COLUMN_GROUP;
					if (!cell.IsEmpty && cell.Number == _selectedCell.Number)
					{
						state = State.SAME_WRONG_NUMBER_IN_ROW_COLUMN_GROUP;
					}
				}

				if (cell.Index == _selectedCellIndex)
				{
					state = State.SELECTED;
				}

				_cellDisplayDataList[cell.Index] = new CellDisplayData(cell.Row,
					cell.Column,
					cell.GroupBox,
					state,
					cell.IsFilledGood,
					cell.Number,
					cell.IsEmpty,
					cell is SolvedByGeneratorCell);
			}
			/*foreach (ICell cell in SudokuBoard.CellsArray)
			{
				_cellDisplayDataList[cell.Index] = new CellDisplayData(cell.Row, cell.Column, State.DEFAULT, cell.IsFilledGood, cell.Number, cell.IsEmpty);
			}
			
			IEnumerable<ICell> cellsWithSameNumber = SudokuBoard.GetFilledCellsWithSameNumber(_selectedCell.Number);
			foreach (ICell cell in cellsWithSameNumber)
			{
				_cellDisplayDataList[cell.Index] = new CellDisplayData(cell.Row, cell.Column, State.SAME_NUMBER, cell.IsFilledGood, cell.Number, cell.IsEmpty);
			}
			
			for (int i = 0; i < SudokuBoard.CellsArray.GetRowsLength(); i++)
			{
				ICell cell = SudokuBoard.CellsArray[i, _selectedCell.Column];
				State state = State.SAME_ROW_COLUMN_GROUP;
				if (cell.Number == _selectedCell.Number && !cell.IsEmpty)
				{
					state = State.SAME_WRONG_NUMBER_IN_ROW_COLUMN_GROUP;
				}
				_cellDisplayDataList[cell.Index] = new CellDisplayData(cell.Row, cell.Column, state, cell.IsFilledGood, cell.Number, cell.IsEmpty);
			}
			
			for (int i = 0; i < SudokuBoard.CellsArray.GetColumnsLength(); i++)
			{
				ICell cell = SudokuBoard.CellsArray[_selectedCell.Row, i];
				State state = State.SAME_ROW_COLUMN_GROUP;
				if (cell.Number == _selectedCell.Number && !cell.IsEmpty)
				{
					state = State.SAME_WRONG_NUMBER_IN_ROW_COLUMN_GROUP;
				}
				_cellDisplayDataList[cell.Index] = new CellDisplayData(cell.Row, cell.Column, state, cell.IsFilledGood, cell.Number, cell.IsEmpty);
			}
			
			foreach (ICell cell in SudokuBoard.GetCellsWithSameGroupBox(_selectedCell.GroupBox))
			{
				State state = State.SAME_ROW_COLUMN_GROUP;
				if (cell.Number == _selectedCell.Number && !cell.IsEmpty)
				{
					state = State.SAME_WRONG_NUMBER_IN_ROW_COLUMN_GROUP;
				}
				_cellDisplayDataList[cell.Index] = new CellDisplayData(cell.Row, cell.Column, state, cell.IsFilledGood, cell.Number, cell.IsEmpty);
			}

			_cellDisplayDataList[_selectedCell.Index] = new CellDisplayData(_selectedCell.Row, _selectedCell.Column, State.SELECTED, _selectedCell.IsFilledGood, _selectedCell.Number, _selectedCell.IsEmpty);*/
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
	public readonly int GroupBox;
	public readonly State State;
	public readonly bool IsFilledGood;
	public readonly int Number;
	public readonly bool AutoGeneratedCell;

	public string Num => _isEmpty ? string.Empty : Number.ToString();

	private readonly bool _isEmpty;

	public CellDisplayData(int row,
		int column,
		int groupBox,
		State state,
		bool isFilledGood,
		int number,
		bool isEmpty,
		bool autoGeneratedCell)
	{
		Row = row;
		Column = column;
		GroupBox = groupBox;
		State = state;
		IsFilledGood = isFilledGood;
		Number = number;
		AutoGeneratedCell = autoGeneratedCell;

		_isEmpty = isEmpty;
	}
}