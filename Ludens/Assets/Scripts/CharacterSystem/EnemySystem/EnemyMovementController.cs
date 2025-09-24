using System.Collections;
using CharacterSystem.PlayerSystem;
using Containers;
using UnityEngine;

namespace CharacterSystem.EnemySystem
{
	public class EnemyMovementController : MonoBehaviour
	{
		private Enemy enemy;
		private float moveSpeed;
		private float rotationSpeed;
		private Player player;

		
		public float MoveSpeed => moveSpeed;
		public float RotationSpeed => rotationSpeed;
		public Enemy Enemy => enemy;
		public int IndexInManager { get; set; }

		public void Initialize(Enemy enemy, int indexInManager)
		{
			this.enemy = enemy;
			var stats = enemy.Stats;
			this.moveSpeed = stats.moveSpeed;
			this.rotationSpeed = stats.rotationSpeed;
			player = Player.Instance;

			IndexInManager = indexInManager;

			enemy.SetState(CharacterState.Moving);
		}

		
	}
}