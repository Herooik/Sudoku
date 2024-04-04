using Gui.Gameplay.Presenters.Cells;
using UnityEngine;

namespace Gui.ScriptableObjects
{
	[CreateAssetMenu(fileName = nameof(SudokuCellsSpawner), menuName = "Game/" + nameof(SudokuCellsSpawner))]
	public class SudokuCellsSpawner : ScriptableObject
	{
		[SerializeField] private SolvedByGeneratorCellPresenter _solvedByGeneratorCellPresenter;
		[SerializeField] private CellForUserPresenter _cellForUserPresenter;

		public ICellPresenter SpawnCell(ICell cell, Transform container)
		{
			return cell switch
			{
				SolvedByGeneratorCell => Instantiate(_solvedByGeneratorCellPresenter, container),
				CellForUser => Instantiate(_cellForUserPresenter, container),
				_ => null
			};
		}
	}
}
