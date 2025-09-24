using CharacterSystem;
using Containers;
using Scriptables;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerSystem
{
	public class Player : CharacterBase
	{
		public static Player Instance { get; private set; }

		public PlayerState State { get; private set; } = PlayerState.Idle;
		public event UnityAction OnStateChanged;

		public PlayerMovementController MovementController { get; private set; }
		public PlayerAttackController AttackController { get; private set; }

		public bool IsMoving => State == PlayerState.Moving;
		public bool IsAttacking => State == PlayerState.Attacking;

		protected override void Awake()
		{
			base.Awake();

			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;

			MovementController = GetComponent<PlayerMovementController>();
			AttackController = GetComponent<PlayerAttackController>();

			if (AttackController) AttackController.Initialize(this);
			if (MovementController) MovementController.Initialize(this);

			OnHealthChanged += UpdateUI;
			OnDied += OnPlayerDied;
		}

		public void SetState(PlayerState newState)
		{
			State = newState;
			OnStateChanged?.Invoke();
		}

		private void UpdateUI(int currentHealth)
		{
			// UI güncellemesi burada yapılır
		}

		private void OnPlayerDied()
		{
			// Player özel ölüm davranışı
		}
	}
}