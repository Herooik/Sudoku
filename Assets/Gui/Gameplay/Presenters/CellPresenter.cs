using System;
using Board;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gui.Gameplay.Presenters
{
	public class CellPresenter : MonoBehaviour
	{
		public RectTransform RectTransform
		{
			get
			{
				if (_rectTransform != null)
					return _rectTransform;
				_rectTransform = GetComponent<RectTransform>();
				return _rectTransform;
			}
		}

		public Cell Cell => _cell;

		[SerializeField] private TextMeshProUGUI _valueText;
		[SerializeField] private Button _button;
		[SerializeField] private Image _image;

		private Cell _cell;
		private RectTransform _rectTransform;

		public void Setup(Cell cell, Action<CellPresenter> onClick)
		{
			_cell = cell;
			_button.onClick.AddListener(() =>
			{
				onClick?.Invoke(this);
			});

			Refresh();
		}

		private void Refresh()
		{
			_valueText.SetText(_cell.IsEmpty() ? string.Empty : _cell.ActualValue.ToString());
		}

		public void Deselect()
		{
			_image.color = Color.white;
		}

		public void Select()
		{
			_image.color = Color.cyan;
		}

		public void ShowSameCellNumber()
		{
			_image.color = Color.gray;
		}

		public void ShowSameRowAndColumn()
		{
			_image.color = Color.yellow;
		}
	}
}
