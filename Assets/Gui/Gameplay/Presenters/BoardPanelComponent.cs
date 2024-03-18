using System.Collections.Generic;
using Board;
using UnityEngine;

namespace Gui.Gameplay.Presenters
{
	public class BoardPanelComponent : MonoBehaviour
	{
		[SerializeField] private RectTransform _holder;
		[SerializeField] private GameObject _cellObj;

		private readonly List<CellPresenter> _cellPresenters = new();
		private int _columns = 9;
		private int _rows = 9;

		public void Initialize(SudokuBoard sudokuBoard)
		{
			float width = _holder.rect.width / _columns;

			_cellPresenters.Clear();
			foreach (Cell cell in sudokuBoard.Cells)
			{
				int row = cell.CellPosition.Row;
				int column = cell.CellPosition.Column;

				GameObject cellObj = Instantiate(_cellObj, _holder);
				cellObj.name = $"[{row}, {column}]";

				CellPresenter cellPresenter = cellObj.GetComponent<CellPresenter>();

				cellPresenter.RectTransform.anchorMin = new Vector2(0.5f, 1);
				cellPresenter.RectTransform.anchorMax = new Vector2(0.5f, 1);
				cellPresenter.RectTransform.sizeDelta = Vector2.one * width;

				float posX = column * width + (_holder.rect.x + width / 2);
				float posY = -(row * width + width / 2);
				cellPresenter.RectTransform.anchoredPosition = new Vector2(posX, posY);

				cellPresenter.Setup(cell, OnClick);
				_cellPresenters.Add(cellPresenter);
			}

			OnClick(_cellPresenters[Random.Range(0, _cellPresenters.Count)]);
		}

		private CellPresenter _selectedCell;

		private void OnClick(CellPresenter selectedCell)
		{
			foreach (CellPresenter cellPresenter in _cellPresenters)
			{
				// todo: consider to move condition to CellPresenter.cs
				if (!cellPresenter.Cell.IsEmpty())
				{
					if (cellPresenter.Cell.Value == selectedCell.Cell.Value)
					{
						cellPresenter.ShowAsDuplicate();
					}
					else
					{
						cellPresenter.Deselect();
					}
				}
			}

			if (_selectedCell != null)
				_selectedCell.Deselect();

			_selectedCell = selectedCell;
			_selectedCell.Select();
		}
	}
}