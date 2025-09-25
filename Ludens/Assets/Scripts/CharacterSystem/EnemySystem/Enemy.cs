using Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace CharacterSystem.EnemySystem
{
	public class Enemy : CharacterBase
	{
		public EnemyMovementController MovementController { get; private set; }
		public EnemyAttackController AttackController { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			MovementController = GetComponent<EnemyMovementController>();
			if (MovementController == null)
			{
				MovementController = transform.AddComponent<EnemyMovementController>();
			}

			AttackController = GetComponent<EnemyAttackController>();
			if (AttackController == null)
			{
				AttackController = transform.AddComponent<EnemyAttackController>();
			}
		}

		private void Start()
		{
			EnemyManager.Instance.RegisterEnemy(MovementController, AttackController, this);
		}

		protected override void HandleDeath()
		{
			EnemyManager.Instance.UnregisterEnemy(MovementController, AttackController,this);
			base.HandleDeath();
		}

		public void DeadImmediately()
		{
			TakeDamage(CurrentHealth);
		}
	}
}