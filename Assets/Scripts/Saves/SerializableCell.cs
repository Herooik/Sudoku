using System;

namespace Saves
{
	[Serializable]
	public class SerializableCell
	{
		public CellType CellType;
		public int Index;
		public int GroupBox;
		public int Row;
		public int Column;
		public int Number;
		public bool IsPlacedGood;

		public SerializableCell(CellType cellType, int index, int groupBox, int row, int column, int number, bool isPlacedGood)
		{
			CellType = cellType;
			Index = index;
			GroupBox = groupBox;
			Row = row;
			Column = column;
			Number = number;
			IsPlacedGood = isPlacedGood;
		}
	}
}