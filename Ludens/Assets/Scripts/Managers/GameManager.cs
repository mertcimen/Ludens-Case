using Controllers;
using UnityEngine;

namespace Managers
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance;

		public InputController inputController;
		public GameStateManager gameStateManager;
		
		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
		}
	}
}