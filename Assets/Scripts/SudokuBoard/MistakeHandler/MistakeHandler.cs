namespace SudokuBoard.MistakeHandler
{
	public class MistakeHandler // todo change name?
	{
		public int Current { get; private set; }
		public int Max { get; private set; }

		public bool MaxedOut => Current >= Max;

		public MistakeHandler(int current, int max)
		{
			Current = current;
			Max = max;
		}

		public void Increase()
		{
			Current++;
		}
	}
}