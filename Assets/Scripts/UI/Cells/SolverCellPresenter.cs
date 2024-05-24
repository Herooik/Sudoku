using System;
using UI.Gameplay;

namespace UI.Cells
{
	public class SolverCellPresenter : CellPresenterBase
	{
		public override void OnSpawned(CellDisplayData cellDisplayData, Action onSelectCell)
		{
			base.OnSpawned(cellDisplayData, onSelectCell);

			_valueText.SetText(cellDisplayData.Num);
		}
	}
}