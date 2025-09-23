using UnityEngine;
using UnityEngine.Events;

namespace PlayerSystem
{
	public class Health : MonoBehaviour
	{
		[SerializeField] private int maxHealth = 100;
		private int currentHealth;

		public int CurrentHealth => currentHealth;
		public int MaxHealth => maxHealth;

		public UnityAction<int> OnHealthChanged;
		public UnityAction OnDied;

		private void Awake()
		{
			currentHealth = maxHealth;
		}

		public void TakeDamage(int amount)
		{
			currentHealth -= amount;
			currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

			OnHealthChanged?.Invoke(currentHealth);

			if (currentHealth <= 0)
			{
				Die();
			}
		}

		public void Heal(int amount)
		{
			currentHealth += amount;
			currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
			OnHealthChanged?.Invoke(currentHealth);
		}

		private void Die()
		{
			OnDied?.Invoke();
		}
	}
}