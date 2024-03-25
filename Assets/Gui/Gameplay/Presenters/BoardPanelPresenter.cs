using System;
using System.Collections.Generic;
using Board;
using UnityEngine;

namespace Gui.Gameplay.Presenters
{
	public class BoardPanelPresenter : MonoBehaviour
	{
		[SerializeField] private RectTransform _holder;
		[SerializeField] private GameObject _cellObj;

		private readonly List<CellPresenter> _cellPresenters = new();

		public void Initialize(int columns, IEnumerable<Cell> cells, Action<Cell> onSelectCell)
		{
			// todo instantiate groupboxes and then cells inside them
			// we can control then groupbox to check if there is a a value etc.

			float width = _holder.rect.width / columns;

			_cellPresenters.Clear();
			foreach (Cell cell in cells)
			{
				int row = cell.CellPosition.Row;
				int column = cell.CellPosition.Column;

				GameObject cellObj = Instantiate(_cellObj, _holder); // todo create a factory
				cellObj.name = $"[{row}, {column}]";

				CellPresenter cellPresenter = cellObj.GetComponent<CellPresenter>();

				cellPresenter.RectTransform.anchorMin = new Vector2(0.5f, 1);
				cellPresenter.RectTransform.anchorMax = new Vector2(0.5f, 1);
				cellPresenter.RectTransform.sizeDelta = Vector2.one * width;

				float posX = column * width + (_holder.rect.x + width / 2);
				float posY = -(row * width + width / 2);
				cellPresenter.RectTransform.anchoredPosition = new Vector2(posX, posY);

				cellPresenter.Setup(cell, () => onSelectCell?.Invoke(cell)); // todo add IDisposable
				_cellPresenters.Add(cellPresenter);
			}
		}

		public void Refresh(Cell selectedCell)
		{
			//todo refactor this
			_cellPresenters[selectedCell.Index].Refresh();
			foreach (CellPresenter cellPresenter in _cellPresenters)
			{
				cellPresenter.Deselect();
			}

			foreach (CellPresenter cellPresenter in _cellPresenters)
			{
				if (!cellPresenter.Cell.IsEmpty())
				{
					if (cellPresenter.Cell.ActualValue == selectedCell.ActualValue)
					{
						cellPresenter.ShowSameCellNumber();
					}
				}
			}

			foreach (CellPresenter cellPresenter in _cellPresenters)
			{
				if (cellPresenter.Cell.CellPosition.Row == selectedCell.CellPosition.Row ||
				    cellPresenter.Cell.CellPosition.Column == selectedCell.CellPosition.Column)
				{
					cellPresenter.ShowSameRowAndColumn();
				}
			}

			_cellPresenters[selectedCell.Index].Select();
		}
	}
}