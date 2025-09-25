using Containers;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterSystem.Managers
{
	public class GameStateManager : MonoBehaviour
	{
		public static GameStateManager Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
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

		[Header("Debug")]
		[SerializeField] private GameState gameState = GameState.None;
		public static event UnityAction<GameState> OnStateChanged;
	}
}