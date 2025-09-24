using PlayerSystem;
using UnityEngine;

namespace CharacterSystem.EnemySystem
{
	public class Enemy : CharacterBase
	{
		protected override void HandleDeath()
		{
			base.HandleDeath();
			// Enemy-specific: loot drop, puan verme, vb.
		}
	}
}