using UnityEngine;

namespace Scriptables
{
	[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Player Data", order = 0)]
	public class PlayerDataSO : ScriptableObject
	{
		[Header("Health")]
		public int maxHealth = 100;

		[Header("Movement")]
		public float moveSpeed = 5f;
		public float rotationSpeed = 10f;

		[Header("Attack")]
		public float attackRange = 5f;
		public float attackRate = 1f;
		public int damage = 10;
		
		public float bulletSpeed = 10f;
		public float bulletLifeTime = 3f;
	}
}