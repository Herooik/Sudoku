using UI.Gameplay;

namespace UI.Cells
{
	public class UserCellPresenter : CellPresenterBase
	{
		public override void Refresh(CellDisplayData cellDisplayData)
		{
			base.Refresh(cellDisplayData);

			_valueText.SetText(cellDisplayData.Num);

			_valueText.color = cellDisplayData.IsFilledGood
				? _cellColorsSetting._goodNumberText
				: _cellColorsSetting._wrongNumberText;
		}
	}
}