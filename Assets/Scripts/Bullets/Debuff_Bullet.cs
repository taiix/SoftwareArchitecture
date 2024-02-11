using UnityEngine;

/// <summary>
/// Represents a debuff bullet fired by a tower that applies a slowing effect to enemies upon hit.
/// </summary>
public class Debuff_Bullet : Bullet
{
    public float slowSpeed;
    [SerializeField] private float slowDuration;

    /// <summary>
    /// Overrides the HitTarget method to apply damage and a slowing effect to the target enemy.
    /// </summary>
    protected override void HitTarget()
    {
        if (target.TryGetComponent<IDamagable>(out IDamagable enemy))
        {
            enemy.Damage(damage);
            if (target.TryGetComponent<Enemy>(out Enemy enemySpeed))
            {
                SlowDebuffCommand slowDebuff = new SlowDebuffCommand(enemySpeed, enemySpeed.speed, slowSpeed, slowDuration);
                slowDebuff.Execute();
            }
        }
        Destroy(this.gameObject);
    }
}
