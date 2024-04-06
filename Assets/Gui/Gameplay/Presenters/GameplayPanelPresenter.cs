using Gui.Gameplay.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Gui.Gameplay.Presenters
{
	public class GameplayPanelPresenter : MonoBehaviour
	{
		[SerializeField] private BoardPanelPresenter _boardPanelPresenter;
		[SerializeField] private PlayerNumberPlacementPresenter _playerNumberPlacementPresenter;
		[SerializeField] private Button _autoSolveButton;
		[SerializeField] private Button _cleanButton;

		private GameplayPanelModel _model;

		public void Bind(GameplayPanelModel model)
		{
			_model = model;

			_boardPanelPresenter.Initialize(_model.SelectCell, _model.CellDisplayDataList);
			_playerNumberPlacementPresenter.Initialize(_model.AllNumbers, _model.PlaceNumber);
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
			_boardPanelPresenter.Refresh(_model.CellDisplayDataList);
			_playerNumberPlacementPresenter.Refresh(_model.AvailableNumbers);
		}
	}
}