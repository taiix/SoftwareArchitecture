using UnityEngine;

/// <summary>
/// Represents a normal bullet
/// </summary>
public class NormalBullet : Bullet
{
    /// <summary>
    /// Overrides the HitTarget method to apply damage to the target enemy.
    /// </summary>
    protected override void HitTarget()
    {
        if (target.TryGetComponent<IDamagable>(out IDamagable enemy)) {
            enemy.Damage(damage);
        }
        
        Destroy(this.gameObject);
    }
}
