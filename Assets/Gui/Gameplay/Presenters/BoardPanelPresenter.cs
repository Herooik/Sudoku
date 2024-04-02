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
		[SerializeField] private EmptyCellPresenter _emptyCellPresenter;
		[SerializeField] private SolvedByGeneratorCellPresenter _solvedByGeneratorCellPresenter;
		[SerializeField] private CellFilledByUserInputPresenter _cellFilledByUserInputPresenter;

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

				ICellPresenter cellPresenter = SpawnCell(cell, _holder);
				cellPresenter.OnSpawned(cell, () => _onSelectCell.Invoke(cell));

				cellPresenter.RectTransform.name = $"[{row}, {column}]";

				SetupCellRect(cellPresenter, width, row, column);

				_cellPresenters[row, column] = cellPresenter;
			}
		}

		private void SetupCellRect(ICellPresenter cellPresenter, float width, int row, int column)
		{
			cellPresenter.RectTransform.anchorMin = new Vector2(0.5f, 1);
			cellPresenter.RectTransform.anchorMax = new Vector2(0.5f, 1);
			cellPresenter.RectTransform.sizeDelta = Vector2.one * width;

			float posX = column * width + (_holder.rect.x + width / 2);
			float posY = -(row * width + width / 2);
			cellPresenter.RectTransform.anchoredPosition = new Vector2(posX, posY);
		}

		private ICellPresenter SpawnCell(ICell cell, Transform container)
		{
			return cell switch
			{
				EmptyCell => Instantiate(_emptyCellPresenter, container),
				SolvedByGeneratorCell => Instantiate(_solvedByGeneratorCellPresenter, container),
				CellForUser => Instantiate(_cellFilledByUserInputPresenter, container),
				_ => null
			};
		}

		public void PlaceNewNumber(ICell selectedCell)
		{
			ICellPresenter temp = _cellPresenters[selectedCell.Row, selectedCell.Column]; // todo change name
			Destroy(temp.RectTransform.gameObject);

			ICellPresenter cellPresenter = SpawnCell(selectedCell, _holder);
			cellPresenter.OnSpawned(selectedCell, () => _onSelectCell.Invoke(selectedCell));

			cellPresenter.RectTransform.name = $"[{selectedCell.Row}, {selectedCell.Column}]";

			float width = _holder.rect.width / _sudokuBoard.GetColumnsLength();

			SetupCellRect(cellPresenter, width, selectedCell.Row, selectedCell.Column);

			_cellPresenters[selectedCell.Row, selectedCell.Column] = cellPresenter;
		}

		public void Refresh(ICell selectedCell)
		{
			foreach (ICellPresenter cellPresenter in _cellPresenters)
			{
				cellPresenter.Deselect();
			}

			for (int i = 0; i < _sudokuBoard.GetRowsLength(); i++)
			{
				_cellPresenters[i, selectedCell.Column].SelectSameColumn();
			}
			for (int i = 0; i < _sudokuBoard.GetColumnsLength(); i++)
			{
				_cellPresenters[selectedCell.Row, i].SelectSameColumn();
			}

			foreach (ICell cell in _sudokuBoard.GetCellsWithSameGroupBox(selectedCell.GroupBox))
			{
				_cellPresenters[cell.Row, cell.Column].SelectSameColumn();
			}

			if (selectedCell is ICellNumber cellNumber)
			{
				IEnumerable<ICell> cellsWithSameNumber = _sudokuBoard.GetCellsWithSameNumber(cellNumber.Number);
				foreach (ICell cell in cellsWithSameNumber)
				{
					_cellPresenters[cell.Row, cell.Column].ShowSameNumber();
				}
			}

			_cellPresenters[selectedCell.Row, selectedCell.Column].Select();
		}
	}
}