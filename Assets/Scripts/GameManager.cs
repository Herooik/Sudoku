using BoardGenerator;
using Configs;
using ScriptableObjects;
using Solver;
using UI.Gameplay;
using UI.Gameplay.Presenters;
using UI.Menu;
using UI.Menu.Presenters;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Transform _panelHolder;
	[SerializeField] private GameplayPanelPresenter _gameplayPanelPresenter;
	[SerializeField] private MainMenuPanelPresenter _mainMenuPanelPresenter;
	[SerializeField] private DifficultyRulesSettings _difficultyRulesSettings;

	private SelectedGameSettings _selectedGameSettings = new();
	private readonly SaveManager _saveManager = new();

	private GameObject _currentPanel;

	private void Start()
	{
		_saveManager.Initialize();
		OpenMainMenuPanel();
	}

	public void OpenMainMenuPanel()
	{
		if (_currentPanel != null)
			Destroy(_currentPanel);

		MainMenuPanelModel model = new MainMenuPanelModel(this, _selectedGameSettings, _saveManager);
		MainMenuPanelPresenter panel = Instantiate(_mainMenuPanelPresenter, _panelHolder);
		panel.GetComponent<MainMenuPanelPresenter>().Bind(model);

		_currentPanel = panel.gameObject;
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
		SaveManager.SaveData saveData = _saveManager.Load();
		_selectedGameSettings = saveData.SelectedGameSettings;

		OpenGameplayPanel(null);
	}

	public void EndGame()
	{
		_saveManager.Delete();
		OpenMainMenuPanel();
	}

	private void OpenGameplayPanel(Board.Board board)
	{
		if (_currentPanel != null)
			Destroy(_currentPanel);

		GameplayPanelModel model = new GameplayPanelModel(this, _selectedGameSettings, _saveManager, board);
		GameplayPanelPresenter panel = Instantiate(_gameplayPanelPresenter, _panelHolder);
		panel.GetComponent<GameplayPanelPresenter>().Bind(model);

		_currentPanel = panel.gameObject;
	}
}