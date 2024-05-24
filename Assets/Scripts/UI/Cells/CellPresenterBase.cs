using System;
using ScriptableObjects;
using TMPro;
using UI.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cells
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

		private RectTransform _rectTransform;

		public virtual void OnSpawned(CellDisplayData cellDisplayData, Action onSelectCell)
		{
			_button.onClick.AddListener(() =>
			{
				onSelectCell?.Invoke();
			});
		}

		public virtual void Refresh(CellDisplayData cellDisplayData)
		{
			switch (cellDisplayData.State)
			{
				case State.DEFAULT:
					_image.color = _cellColorsSetting._default;
					break;
				case State.SELECTED:
					_image.color = _cellColorsSetting._selected;
					break;
				case State.SAME_NUMBER:
					_image.color = _cellColorsSetting._sameNumber;
					break;
				case State.SAME_ROW_COLUMN_GROUP:
					_image.color = _cellColorsSetting._sameColumnRowGroupBox;
					break;
				case State.SAME_WRONG_NUMBER_IN_ROW_COLUMN_GROUP:
					_image.color = _cellColorsSetting._sameWrongNumberInColumnRowGroupBox;
					break;
			}
		}
	}

	public interface ICellPresenter
	{
		RectTransform RectTransform { get; }
		void OnSpawned(CellDisplayData cellDisplayData, Action onSelectCell);
		void Refresh(CellDisplayData cellDisplayData);
	}
}
