using UnityEngine;

namespace CharacterSystem.AttackSystem
{
	public abstract class MeleeAttackBase : MonoBehaviour, IAttackBehaviour
	{
		protected CharacterBase owner;
		[SerializeField] protected float attackRange = 1.5f;
		[SerializeField] protected float attackRate = 1f;
		[SerializeField] protected int damage = 10;

		private float lastAttackTime;

		public virtual void Initialize(CharacterBase owner)
		{
			this.owner = owner;
		}

		public void TryAttack()
		{
			if (Time.time - lastAttackTime < 1f / attackRate) return;

			if (CheckTargetInRange(out var target))
			{
				PerformAttack(target);
				lastAttackTime = Time.time;
			}
		}

		protected abstract void PerformAttack(CharacterBase target);

		protected virtual bool CheckTargetInRange(out CharacterBase target)
		{
			Collider2D[] hits = Physics2D.OverlapCircleAll(owner.transform.position, attackRange);
			foreach (var hit in hits)
			{
				if (hit.TryGetComponent<CharacterBase>(out var character) && character != owner)
				{
					target = character;
					return true;
				}
			}

			target = null;
			return false;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, attackRange);
		}
	}
}