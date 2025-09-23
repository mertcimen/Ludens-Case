using PlayerSystem;
using UnityEngine;

namespace EnemySystem
{
	public class Enemy : MonoBehaviour
	{
		protected Health health;
		
		protected virtual void Awake()
		{
			health = GetComponent<Health>();
			health.OnDied += OnDeath;
		}

		protected virtual void OnDeath()
		{
			gameObject.SetActive(false);
		}

		public virtual void TakeDamage(int amount)
		{
			health.TakeDamage(amount);
		}
	}
}