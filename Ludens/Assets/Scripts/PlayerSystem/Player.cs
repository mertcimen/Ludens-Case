using Containers;
using UnityEngine;

namespace PlayerSystem
{
	public class Player : MonoBehaviour
	{
		public static Player Instance { get; private set; }
		public PlayerState State { get; private set; } = PlayerState.Idle;

		public bool IsMoving => State == PlayerState.Moving;
		public bool IsAttacking => State == PlayerState.Attacking;
		public PlayerMovementController MovementController { get; private set; }

		public void SetState(PlayerState newState)
		{
			State = newState;
			// Debug.Log($"Player state changed to: {newState}");
		}

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;

			MovementController = GetComponent<PlayerMovementController>();
			MovementController.Initialize(this);
		}
	}
}