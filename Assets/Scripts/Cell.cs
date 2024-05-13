public interface ICell
{
	int Index { get; }
	int GroupBox { get; }
	int Row { get; }
	int Column { get; }
	int Number { get; }

	bool IsEmpty => Number <= 0;
	bool IsFilledGood { get; }
	bool IsSolverCell { get; }
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
	public bool IsSolverCell => false;

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

public class UserCell : ICell
{
	public int Index { get; }
	public int GroupBox { get; }
	public int Row { get; }
	public int Column { get; }
	public int Number { get; private set; }

	public bool IsFilledGood => _isGood;
	public bool IsSolverCell => false;

	private readonly int _expectedNumber;
	private bool _isGood;

	public UserCell(int index, int groupBox, int row, int column, int number, int expectedNumber, bool isGood)
	{
		_expectedNumber = expectedNumber;
		_isGood = isGood;

		Index = index;
		GroupBox = groupBox;
		Row = row;
		Column = column;
		Number = number;
	}
}