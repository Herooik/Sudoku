using System;
using Root;
using UnityEngine;

namespace Gui.Menu.Models
{
	public class MainMenuPanelModel : MonoBehaviour
	{
		public event Action Refresh;

		public void StartNewGame()
		{
			FindObjectOfType<ApplicationNavigation>().OpenGameplay();
		}
	}
}
