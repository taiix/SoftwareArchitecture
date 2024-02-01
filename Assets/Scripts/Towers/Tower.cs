using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    protected const float rotationSpeed = 20f;

    [SerializeField] protected TowerInfo type;

    protected int buildingCost;
    public int upgradeCost = 100;

    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float damage;
    [SerializeField] protected int towerLevel;

    private float timer = 0;

    [SerializeField]protected List<GameObject> enemies = new();

    IUpgradable upgradeble;

    private void Start()
    {
        WaveManager.instance.OnEnemyListUpdated += EnemyListHandler;

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

    private void TowerUpgrader(float attackRange, float attackSpeed, float attackDamage, int towerLevel)
    {
        Debug.Log("asdasdasd");
        this.attackRange += attackRange;
        this.attackSpeed += attackSpeed;
        this.damage += attackDamage;
        this.towerLevel += towerLevel;
        this.upgradeCost += upgradeCost;
    }

    private void OnDestroy()
    {
        WaveManager.instance.OnEnemyListUpdated -= EnemyListHandler;
        
        if (upgradeble is MonoBehaviour upgade)
        {
            var updateTower = upgade.GetComponent<Upgrading>();
            updateTower.OnUpgradeUpdate -= TowerUpgrader;
            updateTower.OnCostUpgraded -= TowerCostUpgrader;
        }
    }


    private void EnemyListHandler(List<GameObject> enemies) => this.enemies = enemies;

    private void OnEnable()
    {
        this.buildingCost = type.cost;
        this.attackRange = type.attackRange;
        this.attackSpeed = type.attackSpeed;
        this.damage = type.damage;
    }

    protected void LookAtTarget(GameObject target)
    {
        Vector3 dir = target.transform.position - this.gameObject.transform.position;
        Vector3 targetRot = Quaternion.LookRotation(dir).eulerAngles;

        transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, targetRot.y, 0), 20 * Time.deltaTime);
    }

    protected GameObject FindClosestEnemy()
    {
        float closestDist = float.MaxValue;
        GameObject closestEnemy = null;
        foreach (GameObject enemy in enemies)
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

        GameObject target = FindClosestEnemy();
        LookAtTarget(target);
        timer += Time.deltaTime;

        if (timer % attackSpeed < Time.deltaTime)
        {
            GameObject go = Instantiate(selectBullet, this.gameObject.transform.position, Quaternion.identity);

            if (go.TryGetComponent<Bullet>(out Bullet bullet))
            {
                bullet.FindTarget(target);
                bullet.damage = (int)type.damage;
            }

            Debug.Log(go);
            timer = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.gameObject.transform.position, attackRange);
    }
}
