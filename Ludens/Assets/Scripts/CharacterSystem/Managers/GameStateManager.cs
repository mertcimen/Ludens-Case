using Containers;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterSystem.Managers
{
	public class GameStateManager : MonoBehaviour
	{
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