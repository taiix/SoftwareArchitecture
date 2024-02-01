public interface IUpgradable
{
    void Upgrade(float attackRange, float attackDamage, float attackSpeed, int towerLevel);
    void CostUpdate(int upgradeCost);
}
