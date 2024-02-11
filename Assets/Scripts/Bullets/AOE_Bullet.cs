using UnityEngine;

/// <summary>
/// Represents an area of effect (AOE) bullet fired by a tower.
/// </summary>
public class AOE_Bullet : Bullet
{
    [SerializeField] private float affectedArea;

    /// <summary>
    /// Overrides the HitTarget method to apply damage to enemies within the affected area.
    /// </summary>
    protected override void HitTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, affectedArea);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IDamagable>(out IDamagable enemy))
            {
                enemy.Damage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.gameObject.transform.position, affectedArea);
    }
}
