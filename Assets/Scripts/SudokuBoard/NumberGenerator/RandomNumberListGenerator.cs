using System;
using System.Collections.Generic;

namespace SudokuBoard.NumberGenerator
{
	public class RandomNumberListGenerator
	{
		public IEnumerable<int> GenerateNumbers(int maxNumber)
		{
			List<int> numberList = new();
			for (int i = 1; i <= maxNumber; i++)
			{
				numberList.Add(i);
			}

			Random random = new();
			int n = numberList.Count;
			while (n > 1)
			{
				n--;
				int k = random.Next(n + 1);
				(numberList[k], numberList[n]) = (numberList[n], numberList[k]);
			}

			return numberList;
		}
	}
}