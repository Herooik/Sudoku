public interface ICell
{
	public int Index { get; }
	public int GroupBox { get; }
	public int Row { get; }
	public int Column { get; }
}

public interface ICellNumber
{
	public int Number { get; }
}

public class EmptyCell : ICell
{
	public int Index { get; private set; }
	public int GroupBox { get; private set; }
	public int Row { get; private set; }
	public int Column { get; private set; }

	public EmptyCell(int index, int groupBox, int row, int column)
	{
		Index = index;
		GroupBox = groupBox;
		Row = row;
		Column = column;
	}
}

public class SolvedByGeneratorCell : ICell, ICellNumber
{
	public int Index { get; private set; }
	public int GroupBox { get; private set; }
	public int Row { get; private set; }
	public int Column { get; private set; }
	public int Number { get; private set; }

	public SolvedByGeneratorCell(int index, int groupBox, int row, int column, int number)
	{
		Index = index;
		GroupBox = groupBox;
		Row = row;
		Column = column;
		Number = number;
	}
}

public class CellForUser : ICell, ICellNumber
{
	public int Index { get; private set; }
	public int GroupBox { get; private set; }
	public int Row { get; private set; }
	public int Column { get; private set; }
	public int Number { get; private set; }
	public bool IsPlacedGood => Number == ExpectedNumber;
	public int ExpectedNumber { get; private set; }

	public CellForUser(int index, int groupBox, int row, int column, int number, int expectedNumber)
	{
		Index = index;
		GroupBox = groupBox;
		Row = row;
		Column = column;
		Number = number;
		ExpectedNumber = expectedNumber;
	}
}