using System;
using PlayerSystem;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterSystem
{
	public class Health
	{
		public int CurrentHealth { get; private set; }
		public int MaxHealth { get; private set; }

		public event Action<int> OnHealthChanged;
		public event Action OnDied;

		public Health(int maxHealth)
		{
			MaxHealth = maxHealth;
			CurrentHealth = maxHealth;
		}

		public void TakeDamage(int amount)
		{
			CurrentHealth = Math.Max(CurrentHealth - amount, 0);
			OnHealthChanged?.Invoke(CurrentHealth);

			if (CurrentHealth <= 0)
				OnDied?.Invoke();
		}

		public void Heal(int amount)
		{
			CurrentHealth = Math.Min(CurrentHealth + amount, MaxHealth);
			OnHealthChanged?.Invoke(CurrentHealth);
		}
	}
}