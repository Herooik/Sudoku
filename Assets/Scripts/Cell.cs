﻿public class Cell
{
	public readonly struct Position
	{
		public readonly int Row;
		public readonly int Column;

		public Position(int row, int column)
		{
			Row = row;
			Column = column;
		}
	}

	public int Index { get; private set; }
	public int GroupBox { get; private set; }
	public int ActualValue { get; private set; }
	public Position CellPosition { get; private set; }
	public bool IsRemoved { get; private set; }

	public Cell(int index, int row, int column, int groupBox, int value)
	{
		Index = index;
		GroupBox = groupBox;
		ActualValue = value;

		CellPosition = new Position(row, column);
	}

	public bool IsEmpty()
	{
		return ActualValue <= 0;
	}

	public void SetValue(int value)
	{
		ActualValue = value;
	}

	public void SetAsEmpty()
	{
		IsRemoved = true;
		ActualValue = 0;
	}
}