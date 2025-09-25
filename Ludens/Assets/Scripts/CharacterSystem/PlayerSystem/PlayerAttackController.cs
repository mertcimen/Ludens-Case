using System.Collections;
using Containers;
using DG.Tweening;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;
using WeaponSystem;

namespace CharacterSystem.PlayerSystem
{
	public class PlayerAttackController : MonoBehaviour
	{
		private Player player;
		private Coroutine attackRoutine;
		private Transform currentTarget;

		[SerializeField] private LayerMask enemyLayer;

		// cached stats
		private float attackRange;
		private float attackRate;
		private int damage;

		public void Initialize(Player player)
		{
			this.player = player;
			var stats = player.Stats;
			attackRange = stats.attackRange;
			attackRate = stats.attackRate;
			damage = stats.damage;

			player.OnStateChanged += HandleStateChanged;
		}

		private void Start()
		{
			HandleStateChanged();
		}

		private void HandleStateChanged()
		{
			if (GameManager.Instance.gameStateManager.CurrentState != GameState.Start) return;
			if (player.State == CharacterState.Idle)
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

			while (player != null && player.State == CharacterState.Idle)
			{
				currentTarget = FindClosestEnemyInRange();

				if (currentTarget != null)
				{
					Vector2 dir = (currentTarget.transform.position - transform.position).normalized;
					float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
					transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

					Shoot(currentTarget);

					yield return new WaitForSeconds(1f / attackRate);
				}
				else
				{
					yield return waitCheck;
				}
			}
		}

		private void Shoot(Transform enemy)
		{
			GameObject bulletObj = ObjectPooler.Instance.Spawn("Bullet", transform.position, Quaternion.identity);
			Bullet bullet = bulletObj.GetComponent<Bullet>();
			bullet.Initialize(enemy, "Bullet", damage, player.Stats);
		}

		private Transform FindClosestEnemyInRange()
		{
			Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
			float minDist = Mathf.Infinity;
			Transform closest = null;

			foreach (var hit in hits)
			{
				float dist = (hit.transform.position - transform.position).sqrMagnitude;
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
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, attackRange);
		}
	}
}