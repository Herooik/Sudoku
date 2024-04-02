using System;
using TMPro;
using UnityEngine;

namespace Gui.Gameplay.Presenters.Cells
{
	public class SolvedByGeneratorCellPresenter : CellPresenterBase
	{
		[SerializeField] private TextMeshProUGUI _valueText;

		public override void OnSpawned(ICell cell, Action onSelectCell)
		{
			base.OnSpawned(cell, onSelectCell);

			SolvedByGeneratorCell model = (SolvedByGeneratorCell)cell;
			_valueText.SetText(model.Number.ToString());
		}

		public override void ShowSameNumber()
		{
			base.ShowSameNumber();

			_image.color = new Color(0.25f,.25f, 0, 1);
		}
	}
}