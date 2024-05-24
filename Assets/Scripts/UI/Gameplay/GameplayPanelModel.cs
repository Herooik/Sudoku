﻿using System;
using System.Collections.Generic;
using BoardGenerator;
using Cells;
using Configs;
using PlayerInputNumbers;
using ScriptableObjects;
using Solver;

namespace UI.Gameplay
{
	public class GameplayPanelModel
	{
		public event Action Refresh;

		public int Rows => _sudokuGridConfig.Rows;
		public IReadOnlyList<int> AllNumbers => _inputNumbers.AllNumbers;
		public IEnumerable<int> AvailableNumbers => _inputNumbers.AvailableNumbers;
		public IReadOnlyList<CellDisplayData> CellDisplayDataList => _cellDisplayDataList;
		public SudokuDifficulty Difficulty { get; }
		public int CurrentMistakes => _mistakeHandler.Current;
		public int MaxMistakes => _mistakeHandler.Max;

		private readonly ApplicationNavigation _applicationNavigation;
		private readonly SudokuGridConfig _sudokuGridConfig;
		private readonly Board.Board _board;
		private readonly List<CellDisplayData> _cellDisplayDataList;
		private readonly InputNumbers _inputNumbers;
		private readonly MistakeHandler.MistakeHandler _mistakeHandler;

		private ICell _selectedCell => _board.GetCell(_selectedCellIndex);
		private int _selectedCellIndex;

		public GameplayPanelModel(
			ApplicationNavigation applicationNavigation,
			DifficultyRulesSettings difficultyRulesSettings,
			SelectedGameSettings selectedGameSettings)
		{
			_applicationNavigation = applicationNavigation;
			_sudokuGridConfig = SudokuConfig.GetConfig(selectedGameSettings.SudokuType);

			IBoardSolver boardSolver = new BoardSolver(_sudokuGridConfig);
			_board = new Board.Board(_sudokuGridConfig);
			IBoardGenerator boardGenerator = new RandomBoardGenerator(_sudokuGridConfig, boardSolver, _board.CanPlaceValue, _board.IsFullFilled);
			boardGenerator.Generate(_board);
			RemoveRandomCellsHandler.RemoveRandomCellsFromBoard(_board, difficultyRulesSettings.GetCellsToRemove(selectedGameSettings.SudokuType, selectedGameSettings.Difficulty));

			_cellDisplayDataList = new List<CellDisplayData>(new CellDisplayData[_board.GetRowsLength() * _board.GetRowsLength()]);

			Random random = new();
			_selectedCellIndex = random.Next(0, _cellDisplayDataList.Count);

			_inputNumbers = new InputNumbers(Rows);
			_mistakeHandler = new MistakeHandler.MistakeHandler(0, 3); // todo: move max mistakes to global settings

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

			RefreshState();
		}

		public void PlaceNumber(int number)
		{
			if (_selectedCell.Number == number)
			{
				CleanCell();
				return;
			}

			_board.PlaceValue(number, _selectedCell.Row, _selectedCell.Column);

			if (!_board.GetCell(_selectedCell.Row, _selectedCell.Column).IsFilledGood)
			{
				_mistakeHandler.Increase();
			}

			bool isGameEnd = _board.IsFullFilled() || _mistakeHandler.MaxedOut;
			if (isGameEnd)
			{
				_applicationNavigation.OpenMainMenu();
			}

			RefreshState();
		}

		public void AutoSolveBoard()
		{
			IBoardSolver boardSolver = new ExistedBoardSolver(_sudokuGridConfig);
			boardSolver.Solve(_board, _board.CanPlaceValue, _board.IsFullFilled);

			RefreshState();
		}

		public void CleanCell()
		{
			_board.CleanCell(_selectedCell.Row, _selectedCell.Column);

			RefreshState();
		}

		private void RefreshState()
		{
			RefreshAvailableInputNumbers();
			RefreshCellDisplays();
			Refresh?.Invoke();
		}

		private void RefreshAvailableInputNumbers()
		{
			for (int i = 1; i <= Rows; i++)
			{
				if (_board.IsValueReachMaxOutUsed(i))
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
			for (int row = 0; row < _board.GetRowsLength(); row++)
			{
				for (int col = 0; col < _board.GetRowsLength(); col++)
				{
					ICell cell = _board.GetCell(row, col);
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
						cell is SolverCell);
				}
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
}