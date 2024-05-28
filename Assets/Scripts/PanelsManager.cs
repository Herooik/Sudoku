using Saves;
using UI.Gameplay;
using UI.Gameplay.Presenters;
using UI.Menu;
using UI.Menu.Presenters;
using UnityEngine;

public class PanelsManager : MonoBehaviour
{
	[SerializeField] private Transform _panelHolder;
	[SerializeField] private GameplayPanelPresenter _gameplayPanelPresenter;
	[SerializeField] private MainMenuPanelPresenter _mainMenuPanelPresenter;

	private GameObject _currentPanel;
	private MainMenuPanelModel _mainMenuPanelModel;
	private GameplayPanelModel _gameplayPanelModel;
	private MainMenuPanelPresenter _spawnedMainMenuPanelPresenter;
	private GameplayPanelPresenter _spawnedGameplayPanelPresenter;

	public void Initialize(GameManager gameManager, SelectedGameSettings selectedGameSettings, SaveManager saveManager)
	{
		_gameplayPanelModel = new GameplayPanelModel(gameManager, selectedGameSettings, saveManager);
		_mainMenuPanelModel = new MainMenuPanelModel(gameManager, selectedGameSettings, saveManager.Load());
	}

	public void OpenMainMenuPanel()
	{
		if (_currentPanel != null)
			_currentPanel.SetActive(false);

		_mainMenuPanelModel.Selected();
		if (_spawnedMainMenuPanelPresenter == null)
			_spawnedMainMenuPanelPresenter = Instantiate(_mainMenuPanelPresenter, _panelHolder);
		_spawnedMainMenuPanelPresenter.GetComponent<MainMenuPanelPresenter>().Bind(_mainMenuPanelModel);

		_currentPanel = _spawnedMainMenuPanelPresenter.gameObject;
		_currentPanel.SetActive(true);
	}

	public void OpenGameplayPanel(Board.Board board)
	{
		if (_currentPanel != null)
			_currentPanel.SetActive(false);

		_gameplayPanelModel.Selected(new GameplayPanelParameters()
		{
			Board = board,
		});
		if (_spawnedGameplayPanelPresenter == null)
			_spawnedGameplayPanelPresenter = Instantiate(_gameplayPanelPresenter, _panelHolder);
		_spawnedGameplayPanelPresenter.GetComponent<GameplayPanelPresenter>().Bind(_gameplayPanelModel);

		_currentPanel = _spawnedGameplayPanelPresenter.gameObject;
		_currentPanel.SetActive(true);
	}
}