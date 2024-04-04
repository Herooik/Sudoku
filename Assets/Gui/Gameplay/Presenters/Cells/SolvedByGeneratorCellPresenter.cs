using System;

namespace Gui.Gameplay.Presenters.Cells
{
	public class SolvedByGeneratorCellPresenter : CellPresenterBase
	{
		public override void OnSpawned(ICell cell, Action onSelectCell)
		{
			base.OnSpawned(cell, onSelectCell);

			_valueText.SetText(Cell.Number.ToString());
		}
	}
}