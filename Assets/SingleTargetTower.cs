using UnityEngine;

public class SingleTargetTower : Tower
{
    private float timer = 0;

    public override void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        if (FindClosestEnemy() == null) return;

        GameObject target = FindClosestEnemy();
        LookAtTarget(target);
        timer += Time.deltaTime;

        if (timer % attackSpeed < Time.deltaTime) {
            Debug.Log("attack " + timer);
            timer = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.gameObject.transform.position, attackRange);
    }    

    GameObject FindClosestEnemy()
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
}
