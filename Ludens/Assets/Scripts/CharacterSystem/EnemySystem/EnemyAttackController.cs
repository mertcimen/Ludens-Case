using CharacterSystem.PlayerSystem;
using UnityEngine;

namespace CharacterSystem.EnemySystem
{
	public class EnemyAttackController : MonoBehaviour
	{
		private Enemy enemy;
		private float attackRange;
		private int damage;

		public float AttackRange => attackRange;
		public float Damage => damage;
		public Enemy Enemy => enemy;

		public void Initialize(Enemy enemy, float attackRange, int damage)
		{
			this.enemy = enemy;
			this.attackRange = attackRange;
			this.damage = damage;
		}

		public void OnAttack(Player player)
		{
			player.TakeDamage(damage);
			enemy.DeadImmediately();
		}
		
		
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, attackRange);
		}
		
	}
}