using UnityEngine;

public class AOE_Bullet : Bullet
{
    [SerializeField] private float affectedArea;

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
}
