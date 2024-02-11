using UnityEngine;

/// <summary>
/// Abstract class representing a tower in the game.
/// </summary>
public abstract class Tower : MonoBehaviour
{
    public TowerInfo type;
    public bool canBeBought;

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

    /// <summary>
    /// Increases the tower's upgrade cost.
    /// </summary>
    /// <param name="_upgradeCost"></param>
    private void TowerCostUpgrader(int _upgradeCost)
    {
        upgradeCost += _upgradeCost;
    }

    /// <summary>
    /// Upgrades the tower's attributes.
    /// </summary>
    /// <param name="attackRange"></param>
    /// <param name="attackDamage"></param>
    /// <param name="attackSpeed"></param>
    /// <param name="towerLevel"></param>
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

    /// <summary>
    /// Initializes tower attributes when enabled.
    /// </summary>
    private void OnEnable()
    {
        this.buildingCost = type.cost;
        this.attackRange = type.attackRange;
        this.attackSpeed = type.attackSpeed;
        this.damage = type.damage;
    }

    /// <summary>
    /// Rotates the tower to face the target enemy.
    /// </summary>
    /// <param name="target"></param>
    protected void LookAtTarget(Enemy target)
    {
        Vector3 dir = target.transform.position - this.gameObject.transform.position;
        Vector3 targetRot = Quaternion.LookRotation(dir).eulerAngles;

        transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, targetRot.y, 0), 20 * Time.deltaTime);
    }

    /// <summary>
    /// Finds the closest enemy within the tower's attack range.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Initiates an attack on the closest enemy using the specified bullet.
    /// </summary>
    /// <param name="selectBullet"></param>
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
