using Gui.Gameplay.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gui.Gameplay.Presenters
{
	public class GameplayPanelPresenter : MonoBehaviour
	{
		[SerializeField] private Button _returnButton;
		[SerializeField] private TextMeshProUGUI _difficultyText;
		[SerializeField] private TextMeshProUGUI _mistakesText;
		[SerializeField] private TextMeshProUGUI _scoreText;
		[SerializeField] private TextMeshProUGUI _timeText;
		[SerializeField] private BoardPanelPresenter _boardPanelPresenter;
		[SerializeField] private InputNumbersPresenter _inputNumbersPresenter;
		[SerializeField] private Button _autoSolveButton;
		[SerializeField] private Button _cleanButton;

		private GameplayPanelModel _model;

		public void Bind(GameplayPanelModel model)
		{
			_model = model;

			_difficultyText.SetText($"Difficulty\n{_model.Difficulty}");

			_returnButton.onClick.AddListener(_model.ReturnToMenu);
			_boardPanelPresenter.Initialize(_model.SelectCell, _model.CellDisplayDataList);
			_inputNumbersPresenter.Initialize(_model.AllNumbers, _model.PlaceNumber);
			_autoSolveButton.onClick.AddListener(_model.AutoSolveBoard);
			_cleanButton.onClick.AddListener(_model.CleanCell);

			_model.Refresh += OnRefresh;
			OnRefresh();
		}

		private void OnDisable()
		{
			_model.Refresh -= OnRefresh;
		}

		private void OnRefresh()
		{
			_mistakesText.SetText($"Mistakes \n {_model.CurrentMistakes} / {_model.MaxMistakes}");
			// _scoreText.SetText($"Score \n {_model.Score}");
			// _timeText.SetText($"Time \n {_model.Time}");

			_boardPanelPresenter.Refresh(_model.CellDisplayDataList);
			_inputNumbersPresenter.Refresh(_model.AvailableNumbers);
		}
	}
}