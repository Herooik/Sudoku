namespace Cells
{
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
}