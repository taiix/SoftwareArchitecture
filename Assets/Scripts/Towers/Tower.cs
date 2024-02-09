using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public TowerInfo type;

    public int upgradeCost = 100;
    protected int buildingCost;

    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float damage;
    [SerializeField] protected int towerLevel;

    private float timer = 0;

    IUpgradable upgradeble;

    private void Start()
    {
        upgradeble = GetComponent<IUpgradable>();
        if(upgradeble is MonoBehaviour upgade)
        {
            var updateTower = upgade.GetComponent<Upgrading>();
            updateTower.OnUpgradeUpdate += TowerUpgrader;
            updateTower.OnCostUpgraded += TowerCostUpgrader;
        }
    }

    private void TowerCostUpgrader(int _upgradeCost)
    {
        upgradeCost += _upgradeCost;
    }

    private void TowerUpgrader(float attackRange, float attackDamage, float attackSpeed, int towerLevel)
    {
        this.attackRange += attackRange;
        this.attackSpeed += attackSpeed;
        this.damage += attackDamage;
        this.towerLevel += towerLevel;
        this.upgradeCost += upgradeCost;
    }

    private void OnDestroy()
    {        
        if (upgradeble is MonoBehaviour upgade)
        {
            var updateTower = upgade.GetComponent<Upgrading>();
            updateTower.OnUpgradeUpdate -= TowerUpgrader;
            updateTower.OnCostUpgraded -= TowerCostUpgrader;
        }
    }

    private void OnEnable()
    {
        this.buildingCost = type.cost;
        this.attackRange = type.attackRange;
        this.attackSpeed = type.attackSpeed;
        this.damage = type.damage;
    }

    protected void LookAtTarget(Enemy target)
    {
        Vector3 dir = target.transform.position - this.gameObject.transform.position;
        Vector3 targetRot = Quaternion.LookRotation(dir).eulerAngles;

        transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, targetRot.y, 0), 20 * Time.deltaTime);
    }

    protected Enemy FindClosestEnemy()
    {
        float closestDist = float.MaxValue;
        Enemy closestEnemy = null;
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            float dist = (enemy.transform.position - this.gameObject.transform.position).magnitude;
            if (dist < closestDist && dist <= attackRange)
            {
                closestDist = dist;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }

    protected void Attack(GameObject selectBullet)
    {
        if (FindClosestEnemy() == null) return;

        Enemy target = FindClosestEnemy();
        LookAtTarget(target);
        timer += Time.deltaTime;
        Debug.Log("atk " + attackSpeed);
        if (timer % attackSpeed < Time.deltaTime)
        {
            GameObject go = Instantiate(selectBullet, this.gameObject.transform.position, Quaternion.identity);

            if (go.TryGetComponent<Bullet>(out Bullet bullet))
            {
                bullet.FindTarget(target);
                bullet.damage = (int)type.damage;
            }

            timer = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.gameObject.transform.position, attackRange);
    }
}
