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

    protected List<GameObject> enemies = new();

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

    protected abstract void Attack();

    public virtual void Update()
    {
        Attack();
    }

    protected void LookAtTarget(GameObject target)
    {
        Vector3 dir = target.transform.position - this.gameObject.transform.position;
        Vector3 targetRot = Quaternion.LookRotation(dir).eulerAngles;

        transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, targetRot.y, 0), 20 * Time.deltaTime);
    }
}
