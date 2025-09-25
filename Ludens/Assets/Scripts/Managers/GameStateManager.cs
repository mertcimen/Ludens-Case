using Containers;
using Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
	public class GameStateManager : MonoBehaviour
	{
		public static event UnityAction<GameState> OnStateChanged;

		[Header("Debug")]
		[SerializeField] private GameState gameState = GameState.None;

		private void Start()
		{
			GameManager.Instance.inputController.OnScreenTouched += OnGameStart;
		}

		private void OnGameStart()
		{
			CurrentState = GameState.Start;
			GameManager.Instance.inputController.OnScreenTouched -= OnGameStart;
		}

		public GameState CurrentState
		{
			get => gameState;
			set
			{
				gameState = value;
				OnStateChanged?.Invoke(gameState);
			}
		}
	}
}