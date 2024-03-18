using Board;
using Gui.Gameplay.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace Mock
{
	public class TestSudokuGenerator : MonoBehaviour
	{
		[SerializeField] private BoardPanelComponent _boardPanelComponent;
		[SerializeField] private Button _generateNewGridButton;

		private readonly SudokuService _sudokuService = new();

		private void Start()
		{
			_generateNewGridButton.onClick.AddListener(GenerateNewGrid);

			GenerateNewGrid();
		}

		private void GenerateNewGrid()
		{
			SudokuBoard sudokuBoard = _sudokuService.GetSolvableBoard();
			for (int i = 0; i < _boardPanelComponent.transform.childCount; i++)
			{
				Destroy(_boardPanelComponent.transform.GetChild(i).gameObject);
			}
			_boardPanelComponent.Initialize(sudokuBoard);
		}
	}
}
