public interface ICell
{
	public int Index { get; }
	public int GroupBox { get; }
	public int Row { get; }
	public int Column { get; }
	public int Number { get; }

	bool IsEmpty => Number <= 0;
	bool IsFilledGood { get; }
}

public class SolvedByGeneratorCell : ICell
{
	public int Index { get; private set; }
	public int GroupBox { get; private set; }
	public int Row { get; private set; }
	public int Column { get; private set; }
	public int Number { get; private set; }
	public bool IsFilledGood => true; 

	public SolvedByGeneratorCell(int index, int groupBox, int row, int column, int number)
	{
		Index = index;
		GroupBox = groupBox;
		Row = row;
		Column = column;
		Number = number;
	}
}

public class CellForUser : ICell
{
	public int Index { get; private set; }
	public int GroupBox { get; private set; }
	public int Row { get; private set; }
	public int Column { get; private set; }
	public int Number { get; private set; }
	public bool IsFilledGood => Number == _expectedNumber;

	private readonly int _expectedNumber;

	public CellForUser(int index, int groupBox, int row, int column, int number, int expectedNumber)
	{
		Index = index;
		GroupBox = groupBox;
		Row = row;
		Column = column;
		Number = number;
		_expectedNumber = expectedNumber;
	}

	public void FillCell(int value)
	{
		Number = value;
	}

	public void SetEmpty()
	{
		Number = 0;
	}
}