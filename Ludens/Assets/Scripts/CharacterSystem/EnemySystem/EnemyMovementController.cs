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
		private Coroutine moveRoutine;

		public void Initialize(Enemy enemy)
		{
			this.enemy = enemy;
			var stats = enemy.Stats;
			this.moveSpeed = stats.moveSpeed;
			this.rotationSpeed = stats.rotationSpeed;
			player = Player.Instance;

			SetTargetPosition(player.transform.position);
		}

		public void SetTargetPosition(Vector2 position)
		{
			if (moveRoutine != null) StopCoroutine(moveRoutine);
			moveRoutine = StartCoroutine(MoveToPosition());
		}

		private IEnumerator MoveToPosition()
		{
			enemy.SetState(CharacterState.Moving);

			while ((targetPosition - (Vector2)transform.position).sqrMagnitude > 0.0025f)
			{
				Vector2 currentPosition = transform.position;
				Vector2 direction = (targetPosition - currentPosition).normalized;

				transform.position = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

				if (direction.sqrMagnitude > 0.001f)
				{
					float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
					Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
					transform.rotation =
						Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
				}

				yield return null;
			}

			transform.position = targetPosition;
			moveRoutine = null;
			enemy.SetState(CharacterState.Idle);
		}
	}
}