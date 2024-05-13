using System;
using Gui.Gameplay.Models;

namespace Gui.Gameplay.Presenters.Cells
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