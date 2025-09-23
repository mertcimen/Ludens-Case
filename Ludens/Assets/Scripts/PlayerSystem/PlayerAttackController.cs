using System.Collections;
using Containers;
using UnityEngine;
using Utilities;
using WeaponSystem;

namespace PlayerSystem
{
	public class PlayerAttackController : MonoBehaviour
	{
		private Player player;

		[SerializeField] private float attackRange = 5f;
		[SerializeField] private float attackRate = 1f;
		[SerializeField] private int damage = 10;

		[SerializeField] private Bullet bulletPrefab;
		[SerializeField] private LayerMask enemyLayer;

		private Coroutine attackRoutine;
		private Transform currentTarget;

		public void Initialize(Player player)
		{
			this.player = player;
			player.OnStateChanged += HandleStateChanged;
		}

		private void HandleStateChanged()
		{
			if (player.State == PlayerState.Idle)
			{
				if (attackRoutine == null)
					attackRoutine = StartCoroutine(AttackLoop());
			}
			else
			{
				if (attackRoutine != null)
				{
					StopCoroutine(attackRoutine);
					attackRoutine = null;
				}
			}
		}

		private IEnumerator AttackLoop()
		{
			WaitForSeconds waitCheck = new WaitForSeconds(0.2f);

			while (player.State == PlayerState.Idle)
			{
				currentTarget = FindClosestEnemyInRange();

				if (currentTarget != null)
				{
					Shoot(currentTarget);
					yield return new WaitForSeconds(1f / attackRate);
				}
				else
				{
					yield return waitCheck;
				}
			}
		}

		private IEnumerator ShootAtTarget()
		{
			while (currentTarget != null && Vector2.Distance(transform.position, currentTarget.position) <= attackRange)
			{
				Shoot(currentTarget);
				yield return new WaitForSeconds(1f / attackRate);
			}
		}

		private void Shoot(Transform enemy)
		{
			GameObject bulletObj = ObjectPooler.Instance.Spawn("Bullet", transform.position, Quaternion.identity);
			Bullet bullet = bulletObj.GetComponent<Bullet>();
			bullet.Initialize(enemy, "Bullet", damage);
		}

		private Transform FindClosestEnemyInRange()
		{
			Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
			float minDist = Mathf.Infinity;
			Transform closest = null;

			foreach (var hit in hits)
			{
				float dist = Vector2.SqrMagnitude(hit.transform.position - transform.position);
				if (dist < minDist)
				{
					minDist = dist;
					closest = hit.transform;
				}
			}

			return closest;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, attackRange);
		}
	}
}