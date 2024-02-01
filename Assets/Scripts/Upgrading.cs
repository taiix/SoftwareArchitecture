using UnityEngine;
using UnityEngine.Events;

public class Upgrading : MonoBehaviour, IUpgradable
{
    public UnityAction<float, float, float, int> OnUpgradeUpdate;
    public UnityAction<int> OnCostUpgraded;

    public void CostUpdate(int upgradeCost)
    {
        OnCostUpgraded?.Invoke(upgradeCost);
    }

    public void Upgrade(float attackRange, float attackDamage, float attackSpeed, int towerLevel)
    {
        Debug.Log("Upgraded");
        OnUpgradeUpdate?.Invoke(attackRange, attackDamage, attackSpeed, towerLevel);
    }
}
