using System;

namespace Gui.Gameplay.Presenters.Cells
{
	public class SolvedByGeneratorCellPresenter : CellPresenterBase
	{
		public override void OnSpawned(CellDisplayData cellDisplayData, Action onSelectCell)
		{
			base.OnSpawned(cellDisplayData, onSelectCell);

			_valueText.SetText(cellDisplayData.Num);
		}
	}
}