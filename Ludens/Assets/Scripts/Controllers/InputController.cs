using UnityEngine;
using PlayerSystem;

namespace Controllers
{
	public class InputController : MonoBehaviour
	{
		private Player _player;

		private void Start()
		{
			if (Player.Instance != null)
			{
				_player = Player.Instance;
			}
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector2 targetPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

				if (_player != null)
				{
					_player.MovementController.SetTargetPosition(targetPos);
				}
			}
		}
	}
}