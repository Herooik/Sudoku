using System;
using System.Collections.Generic;
using Board;
using Gui.Gameplay.Presenters.Cells;
using Gui.ScriptableObjects;
using UnityEngine;

namespace Gui.Gameplay.Presenters
{
	public class BoardPanelPresenter : MonoBehaviour
	{
		[SerializeField] private RectTransform _holder;
		[SerializeField] private SudokuCellsSpawner _sudokuCellsSpawner;

		private ICellPresenter[,] _cellPresenters;

		public void Initialize(SudokuBoard sudokuBoard, Action<ICell> onSelectCell, CellDisplayData[,] cellDisplays)
		{
			// todo remove sudokuBoard parameter
			// todo instantiate groupboxes and then cells inside them
			// we can control then groupbox to check if there is a a value etc.

			float width = _holder.rect.width / sudokuBoard.CellsArray.GetColumnsLength();

			_cellPresenters = new ICellPresenter[sudokuBoard.CellsArray.GetRowsLength(), sudokuBoard.CellsArray.GetColumnsLength()];

			foreach (ICell cell in sudokuBoard.CellsArray)
			{
				int row = cell.Row;
				int column = cell.Column;

				ICellPresenter cellPresenter = _sudokuCellsSpawner.SpawnCell(cell, _holder);
				cellPresenter.OnSpawned(cellDisplays[row, column], () => onSelectCell.Invoke(cell));

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

		public void Refresh(CellDisplayData[,] cellDisplays)
		{
			foreach (CellDisplayData cellDisplayData in cellDisplays)
			{
				_cellPresenters[cellDisplayData.Row, cellDisplayData.Column].Refresh(cellDisplayData);
			}
		}
	}
}