using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gui.Gameplay.Presenters
{
	public class BoardPanelPresenter : MonoBehaviour
	{
		[SerializeField] private RectTransform _holder;
		[SerializeField] private GameObject _cellObj;

		private readonly List<CellPresenter> _cellPresenters = new();

		public void Initialize(int columns, IEnumerable<CellData> cells, Action<int> onSelectCell)
		{
			// todo instantiate groupboxes and then cells inside them
			// we can control then groupbox to check if there is a a value etc.

			float width = _holder.rect.width / columns;

			_cellPresenters.Clear();
			foreach (CellData cellDisplay in cells)
			{
				int row = cellDisplay.Row;
				int column = cellDisplay.Column;

				GameObject cellObj = Instantiate(_cellObj, _holder); // todo create a factory
				cellObj.name = $"[{row}, {column}]";

				CellPresenter cellPresenter = cellObj.GetComponent<CellPresenter>();

				cellPresenter.RectTransform.anchorMin = new Vector2(0.5f, 1);
				cellPresenter.RectTransform.anchorMax = new Vector2(0.5f, 1);
				cellPresenter.RectTransform.sizeDelta = Vector2.one * width;

				float posX = column * width + (_holder.rect.x + width / 2);
				float posY = -(row * width + width / 2);
				cellPresenter.RectTransform.anchoredPosition = new Vector2(posX, posY);

				cellPresenter.Setup(() => onSelectCell.Invoke(cellDisplay.Index)); // todo add IDisposable
				_cellPresenters.Add(cellPresenter);
			}
		}

		public void Refresh(IReadOnlyList<CellData> allCells)
		{
			for (int i = 0; i < _cellPresenters.Count; i++)
			{
				_cellPresenters[i].Refresh(allCells[i]);
			}
		}
	}
}