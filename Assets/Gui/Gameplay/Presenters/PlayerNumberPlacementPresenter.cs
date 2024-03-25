using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gui.Gameplay.Presenters
{
	public class PlayerNumberPlacementPresenter : MonoBehaviour
	{
		[SerializeField] private GameObject _numberToPlacePrefab;

		private readonly List<GameObject> _numbers = new();

		public void Initialize(IEnumerable<int> allNumbers, Action<int> placeNumber)
		{
			_numbers.Clear();
			foreach (int number in allNumbers)
			{
				GameObject num = Instantiate(_numberToPlacePrefab, transform);
				num.GetComponent<Button>().onClick.AddListener(() => placeNumber.Invoke(number)); // todo add IDisposable
				num.GetComponentInChildren<TextMeshProUGUI>().SetText(number.ToString());
				_numbers.Add(num);
			}
		}

		public void Refresh()
		{
			
		}
	}
}
