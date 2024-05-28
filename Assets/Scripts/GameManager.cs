using System;
using BoardGenerator;
using Configs;
using Saves;
using ScriptableObjects;
using Solver;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private PanelsManager _panelsManager;
	[SerializeField] private DifficultyRulesSettings _difficultyRulesSettings;

	private readonly SelectedGameSettings _selectedGameSettings = new();
	private readonly SaveManager _saveManager = new();

	private GameObject _currentPanel;
	private SaveManager.SaveData _currentSave;

	private void Start()
	{
		_saveManager.Initialize();
		_currentSave = _saveManager.Load();

		_panelsManager.Initialize(this, _selectedGameSettings, _saveManager);

		OpenMainMenuPanel();
	}

	public void OpenMainMenuPanel()
	{
		_panelsManager.OpenMainMenuPanel();
	}

	public void NewGame()
	{
		SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(_selectedGameSettings.SudokuType);
		IBoardSolver boardSolver = new BoardSolver(sudokuGridConfig);
		Board.Board board = new Board.Board(sudokuGridConfig);
		IBoardGenerator boardGenerator = new RandomBoardGenerator(sudokuGridConfig, boardSolver, board.CanPlaceValue, board.IsFullFilled);
		boardGenerator.Generate(board);

		RemoveRandomCellsHandler.RemoveRandomCellsFromBoard(board, _difficultyRulesSettings.GetCellsToRemove(_selectedGameSettings.SudokuType, _selectedGameSettings.Difficulty));

		OpenGameplayPanel(board);
	}

	public void ContinueGame()
	{
		_currentSave = _saveManager.Load();
		_selectedGameSettings.SudokuType = _currentSave.SudokuType;
		_selectedGameSettings.Difficulty = _currentSave.SudokuDifficulty;

		SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(_selectedGameSettings.SudokuType);
		Board.Board board = new Board.Board(sudokuGridConfig);
		IBoardGenerator boardGenerator = new BoardFromSaveFileGenerator(sudokuGridConfig, _currentSave.Cells);
		boardGenerator.Generate(board);

		OpenGameplayPanel(board);
	}

	public void EndGame()
	{
		_saveManager.Delete();
		OpenMainMenuPanel();
	}

	private void OpenGameplayPanel(Board.Board board)
	{
		_panelsManager.OpenGameplayPanel(board);
	}
}