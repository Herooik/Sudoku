using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gui.ScriptableObjects
{
	[CreateAssetMenu(fileName = nameof(DifficultyRulesSettings), menuName = "Game/" + nameof(DifficultyRulesSettings))]
	public class DifficultyRulesSettings : ScriptableObject
	{
		[Serializable]
		public class Setting
		{
			public SudokuType SudokuType;

			public List<DifficultySetting> DifficultySettings;

			[Serializable]
			public class DifficultySetting
			{
				public SudokuDifficulty SudokuDifficulty;
				public int CellsToRemove;
			}
		}

		[SerializeField] private List<Setting> _difficultySettings;

		public int GetCellsToRemove(SudokuType sudokuType, SudokuDifficulty sudokuDifficulty)
		{
			foreach (Setting difficultySetting in _difficultySettings)
			{
				if (difficultySetting.SudokuType == sudokuType)
				{
					return difficultySetting.DifficultySettings.First(d => d.SudokuDifficulty == sudokuDifficulty).CellsToRemove;
				}
			}
			return 0;
		}
	}
}