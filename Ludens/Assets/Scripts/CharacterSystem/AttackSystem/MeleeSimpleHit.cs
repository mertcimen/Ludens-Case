namespace CharacterSystem.AttackSystem
{
	public class MeleeSimpleHit : MeleeAttackBase
	{
		protected override void PerformAttack(CharacterBase target)
		{
			target.TakeDamage(damage);
		}
	}
}