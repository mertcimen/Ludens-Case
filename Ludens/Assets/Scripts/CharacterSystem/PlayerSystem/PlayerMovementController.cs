using System.Collections;
using Containers;
using UnityEngine;

namespace PlayerSystem
{
	public class PlayerMovementController : MonoBehaviour
	{
		private Player player;
		private Coroutine moveRoutine;

		// cached stats
		private float moveSpeed;
		private float rotationSpeed;

		public void Initialize(Player player)
		{
			this.player = player;
			var stats = player.Stats;
			moveSpeed = stats.moveSpeed;
			rotationSpeed = stats.rotationSpeed;
		}

		public void SetTargetPosition(Vector2 position)
		{
			if (moveRoutine != null) StopCoroutine(moveRoutine);
			moveRoutine = StartCoroutine(MoveToPosition(position));
		}

		private IEnumerator MoveToPosition(Vector2 targetPosition)
		{
			player.SetState(PlayerState.Moving);

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
			player.SetState(PlayerState.Idle);
		}
	}
}