using System;
using System.Collections.Generic;
using Configs;
using Gui.ScriptableObjects;
using SudokuBoard.Board;
using SudokuBoard.MistakeHandler;
using SudokuBoard.PlayerInputNumbers;
using SudokuBoard.Solver;

namespace Gui.Gameplay.Models
{
	public class GameplayPanelModel
	{
		public event Action Refresh;

		public int Rows => _sudokuBoard.CellsArray.GetRowsLength();
		public IReadOnlyList<int> AllNumbers => _inputNumbers.AllNumbers;
		public IEnumerable<int> AvailableNumbers => _inputNumbers.AvailableNumbers;
		public IReadOnlyList<CellDisplayData> CellDisplayDataList => _cellDisplayDataList;
		public SudokuDifficulty Difficulty { get; }
		public int CurrentMistakes => _mistakeHandler.Current;
		public int MaxMistakes => _mistakeHandler.Max;

		private readonly ApplicationNavigation _applicationNavigation;
		private readonly SudokuGridConfig _sudokuGridConfig;
		private readonly SudokuBoard.Board.SudokuBoard _sudokuBoard;
		private readonly List<CellDisplayData> _cellDisplayDataList;
		private readonly InputNumbers _inputNumbers;
		private readonly MistakeHandler _mistakeHandler;

		private ICell _selectedCell => _sudokuBoard.GetCell(_selectedCellIndex);
		private int _selectedCellIndex;

		public GameplayPanelModel(
			ApplicationNavigation applicationNavigation,
			DifficultyRulesSettings difficultyRulesSettings,
			SelectedGameSettings selectedGameSettings)
		{
			_applicationNavigation = applicationNavigation;
			_sudokuGridConfig = SudokuConfig.GetConfig(selectedGameSettings.SudokuType);

			IBoardSolver boardSolver = new BoardSolver(_sudokuGridConfig);
			_sudokuBoard = new SudokuBoard.Board.SudokuBoard(_sudokuGridConfig, boardSolver);
			_sudokuBoard.GenerateNewBoard(difficultyRulesSettings.GetCellsToRemove(selectedGameSettings.SudokuType, selectedGameSettings.Difficulty));

			_cellDisplayDataList = new List<CellDisplayData>(new CellDisplayData[_sudokuBoard.CellsArray.Length]);

			Random random = new();
			_selectedCellIndex = _sudokuBoard.CellsArray[random.Next(0, _sudokuGridConfig.Rows), random.Next(0, _sudokuGridConfig.Rows)].Index;

			_inputNumbers = new InputNumbers(_sudokuBoard.CellsArray.GetRowsLength());
			_mistakeHandler = new MistakeHandler(0, 3); // todo: move max mistakes to global settings

			Difficulty = selectedGameSettings.Difficulty;

			RefreshAvailableInputNumbers();
			RefreshCellDisplays();
		}

		public void ReturnToMenu()
		{
			_applicationNavigation.OpenMainMenu();
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
				CleanCell();
				return;
			}

			_sudokuBoard.PlaceValue(number, _selectedCell.Row, _selectedCell.Column);

			if (!_selectedCell.IsFilledGood)
			{
				_mistakeHandler.Increase();
			}

			bool isGameEnd = _sudokuBoard.IsAllCellsFilledGood() || _mistakeHandler.MaxedOut;
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
			IBoardSolver boardSolver = new ExistedBoardSolver(_sudokuGridConfig);
			boardSolver.Solve(_sudokuBoard.CellsArray, _sudokuBoard.CanPlaceValue, _sudokuBoard.IsFullFilled);

			RefreshAvailableInputNumbers();
			RefreshCellDisplays();
			Refresh?.Invoke();
		}

		public void CleanCell()
		{
			_sudokuBoard.CleanCell(_selectedCell.Row, _selectedCell.Column);

			RefreshAvailableInputNumbers();
			RefreshCellDisplays();
			Refresh?.Invoke();
		}

		private void RefreshAvailableInputNumbers()
		{
			for (int i = 1; i <= _sudokuBoard.CellsArray.GetRowsLength(); i++)
			{
				if (_sudokuBoard.IsValueReachMaxOutUsed(i))
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
			foreach (ICell cell in _sudokuBoard.CellsArray)
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
}