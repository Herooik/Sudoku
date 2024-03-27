using System;
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
				if (_rectTransform == null) 
					_rectTransform = GetComponent<RectTransform>();
				return _rectTransform;
			}
		}

		[SerializeField] private TextMeshProUGUI _valueText;
		[SerializeField] private Button _button;
		[SerializeField] private Image _image;

		private RectTransform _rectTransform;

		public void Setup(Action onClick)
		{
			_button.onClick.AddListener(() =>
			{
				onClick?.Invoke();
			});
		}

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
				case CellState.SAME_VALUE_IN_ROW_AND_COLUMN:
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
	}
}
