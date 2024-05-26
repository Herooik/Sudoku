namespace Cells
{
	public class EmptyCell : ICell
	{
		public int Index { get; }
		public int GroupBox { get; }
		public int Row { get; }
		public int Column { get; }
		public int Number { get; }

		public bool IsEmpty => true;
		public bool IsPlacedGood => false;
		public bool IsSolverCell => false;

		public EmptyCell(int index, int groupBox, int row, int column)
		{
			Index = index;
			GroupBox = groupBox;
			Row = row;
			Column = column;
		}
	}
}