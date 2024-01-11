using UnityEngine;

public class Debuff_Bullet : Bullet
{
    public float slowSpeed;
    [SerializeField] private float slowDuration;

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
