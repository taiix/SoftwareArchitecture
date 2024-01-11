using UnityEngine;

public class NormalBullet : Bullet
{
    protected override void HitTarget()
    {
        Debug.Log("BUM");
        if (target.TryGetComponent<IDamagable>(out IDamagable enemy)) {
            enemy.Damage(damage);
        }
        
        Destroy(this.gameObject);
    }
}
