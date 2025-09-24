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

		protected virtual void Awake()
		{
			health = new Health(stats.maxHealth);
			health.OnHealthChanged += (current) => OnHealthChanged?.Invoke(current);
			health.OnDied += HandleDeath;
		}

		public virtual void TakeDamage(int amount)
		{
			health.TakeDamage(amount);
		}

		protected virtual void HandleDeath()
		{
			OnDied?.Invoke();
			// Default behavior: disable object
			gameObject.SetActive(false);
		}

		public int CurrentHealth => health.CurrentHealth;
		public int MaxHealth => health.MaxHealth;
	}
}