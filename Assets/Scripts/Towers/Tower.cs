using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    protected const float rotationSpeed = 20f;

    [SerializeField] protected TowerInfo type;

    protected int cost;

    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackSpeed;
    protected float damage;

    private float timer = 0;

    [SerializeField]protected List<GameObject> enemies = new();

    private void Start() => WaveManager.instance.OnEnemyListUpdated += EnemyListHandler;

    private void OnDestroy() => WaveManager.instance.OnEnemyListUpdated -= EnemyListHandler;

    private void EnemyListHandler(List<GameObject> enemies) => this.enemies = enemies;

    private void OnEnable()
    {
        this.cost = type.cost;
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
