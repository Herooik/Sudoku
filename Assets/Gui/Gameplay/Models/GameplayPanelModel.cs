using System;

namespace Gui.Gameplay.Models
{
	public class GameplayPanelModel
	{
		public event Action Setup;

		public GameplayPanelModel()
		{
		}

		public void Show()
		{
			Setup?.Invoke();
		}
	}
}