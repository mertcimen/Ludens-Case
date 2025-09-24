using UnityEngine;
using UnityEngine.Events;

namespace PlayerSystem
{
	public class Health : MonoBehaviour
	{
		private Player player;
		private int currentHealth;
		private int maxHealth;

		public int CurrentHealth => currentHealth;
		public int MaxHealth => maxHealth;

		public UnityAction<int> OnHealthChanged;
		public UnityAction OnDied;

		public void Initialize(Player player)
		{
			this.player = player;
			maxHealth = player.Stats.maxHealth; // cache
			currentHealth = maxHealth;
		}

		public void TakeDamage(int amount)
		{
			currentHealth -= amount;
			currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

			OnHealthChanged?.Invoke(currentHealth);

			if (currentHealth <= 0)
				OnDied?.Invoke();
		}

		public void Heal(int amount)
		{
			currentHealth += amount;
			currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
			OnHealthChanged?.Invoke(currentHealth);
		}
	}
}