﻿using System.Collections.Generic;
using Cells;
using Configs;
using Saves;
using UnityEngine;

namespace Board
{
	public class Board
	{
		private readonly SudokuGridConfig _sudokuGridConfig;
		private readonly ICell[,] _cellsArray;

		public readonly List<(int, int)> RemovedCells = new();

		public Board(SudokuGridConfig sudokuGridConfig)
		{
			_sudokuGridConfig = sudokuGridConfig;
			_cellsArray = new ICell[sudokuGridConfig.Rows, sudokuGridConfig.Rows];
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
				if (cell.IsPlacedGood && cell.Number == value)
				{
					count++;
				}
			}

			return count == GetRowsLength();
		}

		public void PlaceValue(int value, int cellRow, int cellColumn)
		{
			ICell targetCell = _cellsArray[cellRow, cellColumn];
			if (targetCell.IsSolverCell)
			{
				Debug.LogWarning("CAN'T PLACE NUMBER ON GENERATED CELL");
				return;
			}

			foreach ((int, int) removedCell in RemovedCells)
			{
				if (removedCell.Item1 == targetCell.Index)
				{
					bool good = removedCell.Item2 == value;
					SetCellAsUser(targetCell.Index, targetCell.GroupBox, targetCell.Row, targetCell.Column, value, good);
					break;
				}
			}
		}

		public void CleanCell(int cellRow, int cellColumn)
		{
			ICell targetCell = _cellsArray[cellRow, cellColumn];
			if (targetCell.IsSolverCell)
			{
				Debug.LogWarning("CAN'T CLEAN NUMBER ON GENERATED CELL");
				return;
			}

			SetCellAsEmpty(targetCell.Index, targetCell.GroupBox, targetCell.Row, targetCell.Column);
		}

		public bool CanPlaceValue(int row, int col, int valueToPlace)
		{
			HashSet<(int, int)> rows = new();
			HashSet<(int, int)> columns = new();
			HashSet<(int, int, int)> subGrids = new();
			foreach (ICell cell in _cellsArray)
			{
				if (cell.IsEmpty) continue;
				if (cell.Row == row && cell.Column == col) continue;

				if (!rows.Add((cell.Row, cell.Number)))
				{
					return false;
				}

				if (!columns.Add((cell.Column, cell.Number)))
				{
					return false;
				}

				if (!subGrids.Add((cell.Row / _sudokuGridConfig.SubGridRows,
					    cell.Column / _sudokuGridConfig.SubGridColumns, cell.Number)))
				{
					return false;
				}
			}

			if (!rows.Add((row, valueToPlace)))
			{
				return false;
			}

			if (!columns.Add((col, valueToPlace)))
			{
				return false;
			}

			if (!subGrids.Add((row / _sudokuGridConfig.SubGridRows, col / _sudokuGridConfig.SubGridColumns, valueToPlace)))
			{
				return false;
			}

			return true;
		}

		public bool Validate()
		{
			HashSet<(int, int)> rows = new();
			HashSet<(int, int)> columns = new();
			HashSet<(int, int, int)> subGrids = new();
			foreach (ICell cell in _cellsArray)
			{
				if (cell.IsEmpty) continue;

				if (!rows.Add((cell.Row, cell.Number)))
				{
					return false;
				}

				if (!columns.Add((cell.Column, cell.Number)))
				{
					return false;
				}

				if (!subGrids.Add((cell.Row / _sudokuGridConfig.SubGridRows,
					    cell.Column / _sudokuGridConfig.SubGridColumns, cell.Number)))
				{
					return false;
				}
			}
			return true;
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

		public void SetCellAsUser(int index, int groupBox, int row, int column, int number, bool isGood)
		{
			_cellsArray[row, column] = new UserCell(index, groupBox, row, column, number, isGood);
		}

		public void SetCellAsEmpty(int index, int groupBox, int row, int column)
		{
			_cellsArray[row, column] = new EmptyCell(index, groupBox, row, column);
		}

		public int GetRowsLength()
		{
			return _cellsArray.GetLength(0);
		}

		public List<SerializableCell> GetSerializableCells()
		{
			List<SerializableCell> list = new();

			for (int row = 0; row < GetRowsLength(); row++)
			{
				for (int col = 0; col < GetRowsLength(); col++)
				{
					ICell cell = GetCell(row, col);

					CellType cellType = CellType.USER;
					if (cell.IsSolverCell)
					{
						cellType = CellType.SOLVER;
					}
					else if (cell.IsEmpty)
					{
						cellType = CellType.EMPTY;
					}
	
					list.Add(new SerializableCell(cellType, cell.Index, cell.GroupBox, cell.Row, cell.Column, cell.Number, cell.IsPlacedGood));
				}
			}

			return list;
		}
	}
}