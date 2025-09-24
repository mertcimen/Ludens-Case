using UnityEngine;

namespace Scriptables
{
	[CreateAssetMenu(fileName = "CharacterStats", menuName = "Game/Character Stats", order = 0)]
	public class CharacterStatsSO : ScriptableObject
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

		[Header("Bullet")]
		public float bulletSpeed = 10f;
		public float bulletLifeTime = 3f;
	}
}