using Containers;
using Scriptables;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerSystem
{
	public class Player : MonoBehaviour
	{
		public static Player Instance { get; private set; }

		[SerializeField] private PlayerDataSO stats; 

		public PlayerDataSO Stats => stats; 

		public PlayerState State { get; private set; } = PlayerState.Idle;
		public event UnityAction OnStateChanged;

		private Health health;
		public PlayerMovementController MovementController { get; private set; }
		public PlayerAttackController AttackController { get; private set; }

		public bool IsMoving => State == PlayerState.Moving;
		public bool IsAttacking => State == PlayerState.Attacking;

		private void Awake()
		{
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

			health = GetComponent<Health>();
			health.Initialize(this); 
			health.OnHealthChanged += UpdateUI;
			health.OnDied += OnPlayerDied;
		}

		public void SetState(PlayerState newState)
		{
			State = newState;
			OnStateChanged?.Invoke();
		}

		public void TakeDamage(int amount)
		{
			health.TakeDamage(amount);
		}

		private void UpdateUI(int currentHealth)
		{
		}

		private void OnPlayerDied()
		{
		}
	}
}