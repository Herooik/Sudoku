public interface ICell
{
	int Index { get; }
	int GroupBox { get; }
	int Row { get; }
	int Column { get; }
	int Number { get; }

	bool IsEmpty => Number <= 0;
	bool IsFilledGood { get; }
}

public class EmptyCell : ICell
{
	public int Index { get; }
	public int GroupBox { get; }
	public int Row { get; }
	public int Column { get; }
	public int Number { get; }

	public bool IsEmpty => true;
	public bool IsFilledGood => false;

	public EmptyCell(int index, int groupBox, int row, int column)
	{
		Index = index;
		GroupBox = groupBox;
		Row = row;
		Column = column;
	}
}

public class SolverCell : ICell
{
	public int Index { get; }
	public int GroupBox { get; }
	public int Row { get; }
	public int Column { get; }
	public int Number { get; }

	public bool IsFilledGood => true; 

	public SolverCell(int index, int groupBox, int row, int column, int number)
	{
		Index = index;
		GroupBox = groupBox;
		Row = row;
		Column = column;
		Number = number;
	}
}

public class UserCell : ICell
{
	public int Index { get; }
	public int GroupBox { get; }
	public int Row { get; }
	public int Column { get; }
	public int Number { get; private set; }
	public bool IsFilledGood => Number == _expectedNumber;

	private readonly int _expectedNumber;

	public UserCell(int index, int groupBox, int row, int column, int number, int expectedNumber)
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