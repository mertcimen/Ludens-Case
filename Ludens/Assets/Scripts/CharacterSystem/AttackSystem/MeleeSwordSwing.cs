using System.Collections;
using UnityEngine;

namespace CharacterSystem.AttackSystem
{
	public class MeleeSwordSwing : MeleeAttackBase
	{
		[SerializeField] private float knockbackForce = 3f;
		[SerializeField] private float stunDuration = 1f;

		public override void Initialize(CharacterBase owner)
		{
		}

		private Coroutine attackRoutine;

		public void EnterAttackState()
		{
			if (attackRoutine != null)
				StopCoroutine(attackRoutine);

			attackRoutine = StartCoroutine(AttackLoop());
		}

		public void ExitAttackState()
		{
			if (attackRoutine != null)
			{
				StopCoroutine(attackRoutine);
				attackRoutine = null;
			}
		}

		private IEnumerator AttackLoop()
		{
			while (true)
			{
				foreach (var attack in attackBehaviours)
				{
					attack.TryAttack();
				}

				yield return new WaitForSeconds(0.4f);
			}
		}

		protected override void PerformAttack(CharacterBase target)
		{
			// Alan vuruşu: etraftaki tüm enemy’leri bul
			Collider2D[] hits = Physics2D.OverlapCircleAll(owner.transform.position, attackRange);
			foreach (var hit in hits)
			{
				if (hit.TryGetComponent<CharacterBase>(out var enemy) && enemy != owner)
				{
					enemy.TakeDamage(damage);

					// TODO: KnockBack
					if (hit.attachedRigidbody != null)
					{
						Vector2 dir = (hit.transform.position - owner.transform.position).normalized;
						hit.attachedRigidbody.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
					}
				}
			}
		}
	}

	public interface IStunnable
	{
		void Stun(float duration);
	}
}