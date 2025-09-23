using EnemySystem;
using UnityEngine;
using Utilities;

namespace WeaponSystem
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private float speed = 10f;
		[SerializeField] private float lifeTime = 3f;
		private Vector3 direction;
		private float timer;
		private string poolTag;

		public void Initialize(Transform target, string poolTag, int damage)
		{
			this.poolTag = poolTag;
			timer = 0f;

			if (target != null)
				direction = (target.position - transform.position).normalized;
			else
				direction = Vector3.zero;
		}

		private void Update()
		{
			transform.position += direction * speed * Time.deltaTime;

			timer += Time.deltaTime;
			if (timer >= lifeTime)
			{
				ObjectPooler.Instance.ReturnToPool(poolTag, gameObject);
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.TryGetComponent<Enemy>(out var enemy))
			{
				enemy.TakeDamage(100);
				ObjectPooler.Instance.ReturnToPool(poolTag, gameObject);
			}
		}
	}
}