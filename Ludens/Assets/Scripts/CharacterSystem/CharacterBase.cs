using System.Collections;
using System.Collections.Generic;
using CharacterSystem.AttackSystem;
using Containers;
using PlayerSystem;
using Scriptables;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterSystem
{
	public abstract class CharacterBase : MonoBehaviour
	{
		public CharacterState State { get; protected set; } = CharacterState.Idle;
		[SerializeField] protected CharacterStatsSO stats;
		protected Health health;
		public CharacterStatsSO Stats => stats;
		public UnityAction OnDied;
		public event UnityAction OnStateChanged;
		public UnityAction<int> OnHealthChanged;

		[SerializeField] private List<MonoBehaviour> attackComponents = new List<MonoBehaviour>();
		protected readonly List<IAttackBehaviour> attackBehaviours = new List<IAttackBehaviour>();

		protected virtual void Awake()
		{
			health = new Health(stats.maxHealth);
			health.OnHealthChanged += (current) => OnHealthChanged?.Invoke(current);
			health.OnDied += HandleDeath;
			
			// attackComponents içinden IAttackBehaviour implement edenleri al
			foreach (var comp in attackComponents)
			{
				if (comp is IAttackBehaviour attack)
				{
					attack.Initialize(this);
					attackBehaviours.Add(attack);
				}
			}
		}

		

		public virtual void TakeDamage(int amount)
		{
			health.TakeDamage(amount);
			Debug.Log(CurrentHealth);
		}

		protected virtual void HandleDeath()
		{
			OnDied?.Invoke();
			gameObject.SetActive(false);
		}

		public int CurrentHealth => health.CurrentHealth;
		public int MaxHealth => health.MaxHealth;
	}
}