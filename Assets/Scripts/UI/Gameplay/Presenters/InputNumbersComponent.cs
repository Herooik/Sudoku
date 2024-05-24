using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Gameplay.Presenters
{
	public class InputNumbersComponent : MonoBehaviour
	{
		[SerializeField] private PlacementInputNumber _placementInputNumber;

		private readonly List<PlacementInputNumber> _numbers = new();

		public void Initialize(IReadOnlyList<int> allNumbers, Action<int> placeNumber)
		{
			_numbers.Clear();

			float width = GetComponent<RectTransform>().rect.width / allNumbers.Count();

			for (int index = 0; index < allNumbers.Count; index++)
			{
				int number = allNumbers[index];
				PlacementInputNumber placementInputNumber = Instantiate(_placementInputNumber, transform);
			
				placementInputNumber.Setup(number, () => placeNumber.Invoke(number));
			
				SetupCellRect(placementInputNumber.GetComponent<RectTransform>(), width, index);
			
				_numbers.Add(placementInputNumber);
			}
		}

		public void Refresh(IEnumerable<int> numbers)
		{
			foreach (PlacementInputNumber placementInputNumber in _numbers)
			{
				if (numbers.Contains(placementInputNumber.Number))
				{
					placementInputNumber.gameObject.SetActive(true);
				}
				else
				{
					placementInputNumber.gameObject.SetActive(false);
				}
			}
		}

		private void SetupCellRect(RectTransform rt, float width, int column)
		{
			rt.anchorMin = new Vector2(0.5f, 1);
			rt.anchorMax = new Vector2(0.5f, 1);
			rt.sizeDelta = Vector2.one * width;

			float posX = column * width + (GetComponent<RectTransform>().rect.x + width / 2);
			float posY = -(width / 2);
			rt.anchoredPosition = new Vector2(posX, posY);
		}
	}
}
