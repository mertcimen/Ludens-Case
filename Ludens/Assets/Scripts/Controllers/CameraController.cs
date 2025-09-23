using PlayerSystem;
using UnityEngine;

namespace Controllers
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Transform target;
		[SerializeField] private float sensitivity = 5f;
		[SerializeField] private Vector3 offset = Vector3.back * 10;

		private void Start()
		{
			if (Player.Instance)
			{
				target = Player.Instance.transform;
			}
		}

		private void LateUpdate()
		{
			if (target == null) return;

			Vector3 desiredPosition = target.position + offset;
			Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, sensitivity * Time.deltaTime);

			transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
		}
	}
}