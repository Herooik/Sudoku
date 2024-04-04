using System;
using Gui.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gui.Gameplay.Presenters.Cells
{
	public class CellPresenterBase : MonoBehaviour, ICellPresenter
	{
		public RectTransform RectTransform
		{
			get
			{
				if (_rectTransform == null) 
					_rectTransform = GetComponent<RectTransform>();
				return _rectTransform;
			}
		}

		[SerializeField] private Button _button;
		[SerializeField] protected Image _image;
		[SerializeField] protected TextMeshProUGUI _valueText;
		[SerializeField] protected CellColorsSetting _cellColorsSetting;

		private protected ICell Cell;

		private RectTransform _rectTransform;

		public virtual void OnSpawned(ICell cell, Action onSelectCell)
		{
			Cell = cell;
			_button.onClick.AddListener(() =>
			{
				onSelectCell?.Invoke();
			});
		}

		public void Deselect()
		{
			_image.color = _cellColorsSetting._default;
		}

		public virtual void Select()
		{
			_image.color = _cellColorsSetting._selected;
		}

		public void SelectSameColumn(int selectedCellNumber)
		{
			if (Cell.Number == selectedCellNumber && !Cell.IsEmpty)
			{
				_image.color = _cellColorsSetting._sameWrongNumberInColumnRowGroupBox;
			}
			else
			{
				_image.color = _cellColorsSetting._sameColumnRowGroupBox;
			}
		}

		public void ShowSameNumber()
		{
			_image.color = _cellColorsSetting._sameNumber;
		}
	}

	public interface ICellPresenter
	{
		RectTransform RectTransform { get; }
		void OnSpawned(ICell cell, Action onSelectCell);
		void Deselect();
		void Select();
		void SelectSameColumn(int selectedCellNumber);
		void ShowSameNumber();
	}
}
