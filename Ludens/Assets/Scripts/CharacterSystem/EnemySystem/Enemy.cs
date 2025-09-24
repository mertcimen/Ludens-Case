using Unity.VisualScripting;
using UnityEngine;

namespace CharacterSystem.EnemySystem
{
	public class Enemy : CharacterBase
	{
		public EnemyMovementController MovementController { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			MovementController = GetComponent<EnemyMovementController>();
			if (MovementController == null)
			{
				MovementController = transform.AddComponent<EnemyMovementController>();
			}

			MovementController.Initialize(this);
		}

		protected override void HandleDeath()
		{
			base.HandleDeath();
		}
	}
}