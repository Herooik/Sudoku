using System;
using System.Collections.Generic;
using Configs;
using UnityEngine;

namespace ScriptableObjects
{
	[CreateAssetMenu(fileName = nameof(DifficultyRuleSetting), menuName = "Game/" + nameof(DifficultyRuleSetting))]
	public class DifficultyRuleSetting : ScriptableObject
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
}