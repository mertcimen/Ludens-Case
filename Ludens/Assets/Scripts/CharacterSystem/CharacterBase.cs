using Containers;
using Scriptables;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterSystem
{
	public abstract class CharacterBase : MonoBehaviour
	{
		[SerializeField] protected CharacterStatsSO stats;

		protected Health health;

		public CharacterStatsSO Stats => stats;
		public UnityAction OnDied;
		public UnityAction<int> OnHealthChanged;

		public CharacterState State { get; protected set; } = CharacterState.Idle;
		public event UnityAction OnStateChanged;
		
		public int CurrentHealth => health.CurrentHealth;
		public int MaxHealth => health.MaxHealth;
		
		protected virtual void Awake()
		{
			health = new Health(stats.maxHealth);
			health.OnHealthChanged += (current) => OnHealthChanged?.Invoke(current);
			health.OnDied += HandleDeath;
		}

		public void SetState(CharacterState newState)
		{
			State = newState;
			OnStateChanged?.Invoke();
		}

		
		public virtual void TakeDamage(int amount)
		{
			health.TakeDamage(amount);
		}

		protected virtual void HandleDeath()
		{
			OnDied?.Invoke();
			gameObject.SetActive(false);
		}

		
	}
}