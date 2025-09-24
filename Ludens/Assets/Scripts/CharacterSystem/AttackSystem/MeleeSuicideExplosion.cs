namespace CharacterSystem.AttackSystem
{
	public class MeleeSuicideExplosion : MeleeAttackBase
	{
		
		
		
		protected override void PerformAttack(CharacterBase target)
		{
			target.TakeDamage(damage);
			owner.TakeDamage(owner.CurrentHealth);
		}
	}
}