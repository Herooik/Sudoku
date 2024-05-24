namespace Cells
{
	public class UserCell : ICell
	{
		public int Index { get; }
		public int GroupBox { get; }
		public int Row { get; }
		public int Column { get; }
		public int Number { get; private set; }

		public bool IsFilledGood { get; }
		public bool IsSolverCell => false;

		public UserCell(int index, int groupBox, int row, int column, int number, bool isGood)
		{
			IsFilledGood = isGood;

			Index = index;
			GroupBox = groupBox;
			Row = row;
			Column = column;
			Number = number;
		}
	}
}