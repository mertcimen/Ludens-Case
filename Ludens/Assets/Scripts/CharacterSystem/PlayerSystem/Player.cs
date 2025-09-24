using Containers;
using PlayerSystem;
using Scriptables;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterSystem.PlayerSystem
{
	public class Player : CharacterBase
	{
		public static Player Instance { get; private set; }
		
		public PlayerMovementController MovementController { get; private set; }
		public PlayerAttackController AttackController { get; private set; }
		public bool IsMoving => State == CharacterState.Moving;
		public bool IsAttacking => State == CharacterState.Attacking;

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

			// if (AttackController) AttackController.Initialize(this);
			if (MovementController) MovementController.Initialize(this);

			OnHealthChanged += UpdateUI;
			OnDied += OnPlayerDied;
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