using System;
using TMPro;
using UnityEngine;

namespace Gui.Gameplay.Presenters.Cells
{
	public class CellFilledByUserInputPresenter : CellPresenterBase
	{
		[SerializeField] private TextMeshProUGUI _valueText;

		public override void OnSpawned(ICell cell, Action onSelectCell)
		{
			base.OnSpawned(cell, onSelectCell);

			CellForUser model = (CellForUser)cell;
			_valueText.SetText(model.Number.ToString());

			_valueText.color = model.IsPlacedGood ? Color.blue : Color.red;
		}

		public override void ShowSameNumber()
		{
			base.ShowSameNumber();

			// _image.color = new Color(0.25f,.25f, 0, 1);
			_image.color = Color.green;
		}
	}
}