using Gui.Gameplay.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gui.Gameplay.Presenters
{
	public class GameplayPanelPresenter : MonoBehaviour
	{
		[SerializeField] private BoardPanelPresenter _boardPanelPresenter;
		[SerializeField] private PlayerNumberPlacementPresenter _playerNumberPlacementPresenter;

		private GameplayPanelModel _model;

		public void Bind(GameplayPanelModel model)
		{
			_model = model;

			_boardPanelPresenter.Initialize(_model.SudokuBoard, _model.SelectCell);
			_playerNumberPlacementPresenter.Initialize(_model.AllNumbers, _model.PlaceNumber);

			_model.Refresh += OnRefresh;
			OnRefresh();
		}

		private void OnDisable()
		{
			_model.Refresh -= OnRefresh;
		}

		private void OnRefresh()
		{
			_boardPanelPresenter.Refresh(_model.SelectedCell);
			_playerNumberPlacementPresenter.Refresh(_model.AvailableNumbers);
		}
	}
}