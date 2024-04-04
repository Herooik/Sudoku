using System;
using System.Collections.Generic;
using Board;
using Gui.Gameplay.Presenters.Cells;
using UnityEngine;

namespace Gui.Gameplay.Presenters
{
	public class BoardPanelPresenter : MonoBehaviour
	{
		[SerializeField] private RectTransform _holder;
		[SerializeField] private SudokuCellsSpawner _sudokuCellsSpawner;

		private ICellPresenter[,] _cellPresenters;
		private SudokuBoard _sudokuBoard;
		private Action<ICell> _onSelectCell;

		public void Initialize(SudokuBoard sudokuBoard, Action<ICell> onSelectCell)
		{
			_sudokuBoard = sudokuBoard;
			_onSelectCell = onSelectCell;

			// todo instantiate groupboxes and then cells inside them
			// we can control then groupbox to check if there is a a value etc.

			float width = _holder.rect.width / sudokuBoard.GetColumnsLength();

			_cellPresenters = new ICellPresenter[sudokuBoard.GetRowsLength(), sudokuBoard.GetColumnsLength()];
			foreach (ICell cell in sudokuBoard.CellsArray)
			{
				int row = cell.Row;
				int column = cell.Column;

				ICellPresenter cellPresenter = _sudokuCellsSpawner.SpawnCell(cell, _holder);
				cellPresenter.OnSpawned(cell, () => _onSelectCell.Invoke(cell));

				cellPresenter.RectTransform.name = $"[{row}, {column}]";

				SetupCellRect(cellPresenter.RectTransform, width, row, column);

				_cellPresenters[row, column] = cellPresenter;
			}
		}

		private void SetupCellRect(RectTransform cellPresenterRt, float width, int row, int column)
		{
			cellPresenterRt.anchorMin = new Vector2(0.5f, 1);
			cellPresenterRt.anchorMax = new Vector2(0.5f, 1);
			cellPresenterRt.sizeDelta = Vector2.one * width;

			float posX = column * width + (_holder.rect.x + width / 2);
			float posY = -(row * width + width / 2);
			cellPresenterRt.anchoredPosition = new Vector2(posX, posY);
		}

		public void Refresh(ICell selectedCell)
		{
			foreach (ICellPresenter cellPresenter in _cellPresenters)
			{
				cellPresenter.Deselect();
			}

			IEnumerable<ICell> cellsWithSameNumber = _sudokuBoard.GetFilledCellsWithSameNumber(selectedCell.Number);
			foreach (ICell cell in cellsWithSameNumber)
			{
				_cellPresenters[cell.Row, cell.Column].ShowSameNumber();
			}

			for (int i = 0; i < _sudokuBoard.GetRowsLength(); i++)
			{
				_cellPresenters[i, selectedCell.Column].SelectSameColumn(selectedCell.Number);
			}
			for (int i = 0; i < _sudokuBoard.GetColumnsLength(); i++)
			{
				_cellPresenters[selectedCell.Row, i].SelectSameColumn(selectedCell.Number);
			}

			foreach (ICell cell in _sudokuBoard.GetCellsWithSameGroupBox(selectedCell.GroupBox))
			{
				_cellPresenters[cell.Row, cell.Column].SelectSameColumn(selectedCell.Number);
			}

			_cellPresenters[selectedCell.Row, selectedCell.Column].Select();
		}
	}
}