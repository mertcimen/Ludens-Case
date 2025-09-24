namespace CharacterSystem.AttackSystem
{
	public interface IAttackBehaviour
	{
		void Initialize(CharacterBase owner);
		void TryAttack();
	}
}