using System;

namespace Gui.Gameplay.Presenters.Cells
{
	public class CellForUserPresenter : CellPresenterBase
	{
		public override void OnSpawned(ICell cell, Action onSelectCell)
		{
			base.OnSpawned(cell, onSelectCell);

			CellForUser model = (CellForUser)cell;
			if (Cell.IsEmpty)
			{
				_valueText.SetText(string.Empty);
			}
			else
			{
				_valueText.SetText(Cell.Number.ToString());
				_valueText.color = model.IsFilledGood
					? _cellColorsSetting._goodNumberText
					: _cellColorsSetting._wrongNumberText;
			}
		}

		public override void Select()
		{
			base.Select();

			CellForUser model = (CellForUser)Cell;
			if (Cell.IsEmpty)
			{
				_valueText.SetText(string.Empty);
			}
			else
			{
				_valueText.SetText(Cell.Number.ToString());
				_valueText.color = model.IsFilledGood
					? _cellColorsSetting._goodNumberText
					: _cellColorsSetting._wrongNumberText;
			}
		}
	}
}