using System.Collections.Generic;
using System.Linq;
using Configs;
using SudokuBoard.BoardGenerator;
using SudokuBoard.Solver;
using UnityEngine;

namespace SudokuBoard.Board
{
	public class Board
	{
		private readonly IBoardSolver _boardSolver;
		private readonly SudokuGridConfig _sudokuGridConfig;
		private readonly ICell[,] _cellsArray;

		public Board(SudokuGridConfig sudokuGridConfig, IBoardSolver boardSolver)
		{
			_sudokuGridConfig = sudokuGridConfig;
			_boardSolver = boardSolver;

			_cellsArray = new ICell[sudokuGridConfig.Rows, sudokuGridConfig.Rows];
		}

		public void GenerateNewBoard(int cellsToRemove)
		{
			IBoardGenerator boardGenerator = new RandomBoardGenerator(_sudokuGridConfig, _boardSolver, CanPlaceValue, IsFullFilled);
			boardGenerator.Generate(this);

			// todo move to other place
			RemoveRandomCellsHandler.RemoveRandomCellsFromBoard(_cellsArray, cellsToRemove, SetCellAsEmpty);
		}

		public bool IsFullFilled()
		{
			foreach (ICell cell in _cellsArray)
			{
				if (cell.IsEmpty)
					return false;
			}
			return true;
		}

		public bool IsValueReachMaxOutUsed(int value) //todo CHANGE NAME
		{
			int count = 0;
			foreach (ICell cell in _cellsArray)
			{
				if (cell.IsFilledGood && cell.Number == value)
				{
					count++;
				}
			}
			return count == GetRowsLength();
		}

		public void PlaceValue(int value, int cellRow, int cellColumn)
		{
			ICell targetCell = _cellsArray[cellRow, cellColumn];
			if (targetCell is UserCell cellForUser)
			{
				cellForUser.FillCell(value);
			}
			else
			{
				Debug.LogWarning("CAN'T PLACE NUMBER ON GENERATED CELL");
			}
		}

		public void CleanCell(int cellRow, int cellColumn)
		{
			ICell targetCell = _cellsArray[cellRow, cellColumn];
			if (targetCell is UserCell cellForUser)
			{
				cellForUser.SetEmpty();
			}
			else
			{
				Debug.LogWarning("CAN'T CLEAN NUMBER ON GENERATED CELL");
			}
		}

		public bool CanPlaceValue(int valueToPlace, ICell cellToPlace)
		{
			HashSet<(int, int)> rows = new();
			HashSet<(int, int)> columns = new();
			HashSet<(int, int, int)> subGrids = new();
			foreach (ICell cell in _cellsArray)
			{
				if (cell.IsEmpty) continue;
				if (cell.Row == cellToPlace.Row && cell.Column == cellToPlace.Column) continue;

				if (!rows.Add((cell.Row, cell.Number)))
				{
					return false;
				}

				if (!columns.Add((cell.Column, cell.Number)))
				{
					return false;
				}

				if (!subGrids.Add((cell.Row / _sudokuGridConfig.SubGridRows, cell.Column / _sudokuGridConfig.SubGridColumns, cell.Number)))
				{
					return false;
				}
			}

			if (!rows.Add((cellToPlace.Row, valueToPlace)))
			{
				return false;
			}

			if (!columns.Add((cellToPlace.Column, valueToPlace)))
			{
				return false;
			}

			if (!subGrids.Add((cellToPlace.Row / _sudokuGridConfig.SubGridRows, cellToPlace.Column / _sudokuGridConfig.SubGridColumns, valueToPlace)))
			{
				return false;
			}

			return true;
		}

		public void SetCellAsEmpty(ICell cell)
		{
			ICell temp = _cellsArray[cell.Row, cell.Column];
			_cellsArray[cell.Row, cell.Column] = new UserCell(temp.Index, temp.GroupBox, temp.Row, temp.Column, 0, temp.Number);
		}

		public bool IsAllCellsFilledGood()
		{
			int count = _cellsArray.Cast<ICell>().Count(cell => cell.IsFilledGood);
			return count == _cellsArray.Length;
		}

		public ICell GetCell(int selectedCellIndex)
		{
			foreach (ICell cell in _cellsArray)
			{
				if (cell.Index == selectedCellIndex)
					return cell;
			}
			return null;
		}

		public ICell GetCell(int row, int column)
		{
			return _cellsArray[row, column];
		}

		public void SetCellAsSolver(int index, int groupBox, int row, int column, int number)
		{
			_cellsArray[row, column] = new SolverCell(index, groupBox, row, column, number);
		}

		public void SetCellAsUser(int index, int groupBox, int row, int column, int number, int expectedNumber)
		{
			//todo remove number and expectedNumber param
			_cellsArray[row, column] = new UserCell(index, groupBox, row, column, number, expectedNumber);
		}

		public void SetCellAsEmpty(int index, int groupBox, int row, int column)
		{
			_cellsArray[row, column] = new EmptyCell(index, groupBox, row, column);
		}

		public int GetRowsLength()
		{
			return _cellsArray.GetLength(0);
		}

		public IEnumerable<ICell> GetAllCellsWithNumber(int numberToRemove)
		{
			List<ICell> cells = new();
			foreach (ICell cell in _cellsArray)
			{
				if (cell.Number == numberToRemove)
				{
					cells.Add(cell);
				}
			}
			return cells;
		}
	}
}