using System.Collections.Generic;
using System.Linq;
using Configs;
using UnityEngine;

namespace ScriptableObjects
{
	[CreateAssetMenu(fileName = nameof(DifficultyRulesSettings), menuName = "Game/" + nameof(DifficultyRulesSettings))]
	public class DifficultyRulesSettings : ScriptableObject
	{
		[SerializeField] private List<DifficultyRuleSetting> _settings;

		public int GetCellsToRemove(SudokuType sudokuType, SudokuDifficulty sudokuDifficulty)
		{
			foreach (DifficultyRuleSetting difficultySetting in _settings)
			{
				if (difficultySetting.SudokuType == sudokuType)
				{
					return difficultySetting.DifficultySettings.First(d => d.SudokuDifficulty == sudokuDifficulty).CellsToRemove;
				}
			}
			return 5;
		}
	}
}