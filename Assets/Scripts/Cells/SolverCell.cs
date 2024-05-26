namespace Cells
{
	public class SolverCell : ICell
	{
		public int Index { get; }
		public int GroupBox { get; }
		public int Row { get; }
		public int Column { get; }
		public int Number { get; }

		public bool IsPlacedGood => true; 
		public bool IsSolverCell => true;

		public SolverCell(int index, int groupBox, int row, int column, int number)
		{
			Index = index;
			GroupBox = groupBox;
			Row = row;
			Column = column;
			Number = number;
		}
	}
}