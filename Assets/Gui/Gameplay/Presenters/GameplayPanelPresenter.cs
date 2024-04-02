using Gui.Gameplay.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gui.Gameplay.Presenters
{
	public class GameplayPanelPresenter : MonoBehaviour
	{
		[SerializeField] private BoardPanelPresenter _boardPanelPresenter;
		[FormerlySerializedAs("_numbersToPlacePresenter")] [SerializeField] private PlayerNumberPlacementPresenter _playerNumberPlacementPresenter;

		private GameplayPanelModel _model;

		public void Bind(GameplayPanelModel model)
		{
			_model = model;

			_model.Setup += OnSetup;
			_model.Refresh += OnRefresh;
			_model.PlaceNewNumber += OnPlaceNewNumber;
		}

		private void OnDisable()
		{
			_model.Setup -= OnSetup;
			_model.Refresh -= OnRefresh;
			_model.PlaceNewNumber -= OnPlaceNewNumber;
		}

		private void OnSetup()
		{
			_boardPanelPresenter.Initialize(_model.SudokuBoard, _model.SelectCell);
			_playerNumberPlacementPresenter.Initialize(_model.Numbers, _model.PlaceNumber);

			OnRefresh();
		}

		private void OnRefresh()
		{
			_boardPanelPresenter.Refresh(_model.SelectedCell);
			_playerNumberPlacementPresenter.Refresh();
		}

		private void OnPlaceNewNumber()
		{
			_boardPanelPresenter.PlaceNewNumber(_model.SelectedCell);

			OnRefresh();
		}
	}
}