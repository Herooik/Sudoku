using System;
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

		private RectTransform _rectTransform;

		public virtual void OnSpawned(ICell cell, Action onSelectCell)
		{
			_button.onClick.AddListener(() =>
			{
				onSelectCell?.Invoke();
			});
		}

		public void Deselect()
		{
			_image.color = Color.white;
		}

		public void Select()
		{
			_image.color = Color.cyan;
		}

		public void SelectSameColumn()
		{
			_image.color = Color.yellow;
		}

		public virtual void ShowSameNumber() { }

		// [SerializeField] private TextMeshProUGUI _valueText;

		//
		// public void Setup(int value, Action onClick)
		// {
		// 	_button.onClick.AddListener(() =>
		// 	{
		// 		onClick?.Invoke();
		// 	});
		// 	_valueText.SetText(value.ToString());
		// }

		/*
		public void Refresh(CellData cellData)
		{
			_valueText.SetText(cellData.Value);

			switch (cellData.UserPlacedValue)
			{
				case UserPlacedValue.BY_GENERATOR:
					_valueText.color = Color.black;
					break;
				case UserPlacedValue.USER_PLACED_GOOD:
					_valueText.color = Color.blue;
					break;
				case UserPlacedValue.USER_PLACED_WRONG:
					_valueText.color = Color.red;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			switch (cellData.CellState)
			{
				case CellState.NONE:
					_image.color = Color.white;
					break;
				case CellState.SELECTED:
					_image.color = Color.cyan;
					break;
				case CellState.SAME_ROW_COLUMN:
					_image.color = Color.yellow;
					break;
				case CellState.WRONG_VALUE:
					_image.color = new Color(1,.5f, .5f, 1);
					break;
				case CellState.SAME_VALUE:
					_image.color = new Color(0.25f,.25f, 0, 1);
					break;
				case CellState.SAME_GROUP_BOX:
					_image.color = Color.yellow;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	*/
	}

	public interface ICellPresenter
	{
		RectTransform RectTransform { get; }
		void OnSpawned(ICell cell, Action onSelectCell);
		void Deselect();
		void Select();
		void SelectSameColumn();
		void ShowSameNumber();
	}
}
