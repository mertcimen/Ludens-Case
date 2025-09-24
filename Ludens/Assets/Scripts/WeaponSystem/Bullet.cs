using System.Collections;
using CharacterSystem.EnemySystem;
using Scriptables;
using UnityEngine;
using Utilities;

namespace WeaponSystem
{
	public class Bullet : MonoBehaviour
	{
		private Vector3 direction;
		private string poolTag;

		private float speed;
		private float lifeTime;
		private int damage;

		private Coroutine lifeRoutine;

		public void Initialize(Transform target, string poolTag, int damage, CharacterStatsSO stats)
		{
			this.poolTag = poolTag;
			this.damage = damage;
			this.speed = stats.bulletSpeed;
			this.lifeTime = stats.bulletLifeTime;

			direction = target != null ? (target.position - transform.position).normalized : Vector3.zero;

			if (lifeRoutine != null)
				StopCoroutine(lifeRoutine);

			lifeRoutine = StartCoroutine(LifeCycle());
		}

		WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

		private IEnumerator LifeCycle()
		{
			float timer = 0f;

			while (timer < lifeTime)
			{
				transform.position += direction * speed * Time.fixedDeltaTime;

				timer += Time.fixedDeltaTime;
				yield return waitForFixedUpdate;
			}

			ReturnToPool();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.TryGetComponent<CharacterSystem.CharacterBase>(out var character))
			{
				character.TakeDamage(damage);

				if (lifeRoutine != null)
					StopCoroutine(lifeRoutine);

				ReturnToPool();
			}
		}

		private void ReturnToPool()
		{
			ObjectPooler.Instance.ReturnToPool(poolTag, gameObject);
		}
	}
}