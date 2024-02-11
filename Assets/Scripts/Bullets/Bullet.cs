using UnityEngine;

/// <summary>
/// Abstract class representing a bullet fired by a tower.
/// </summary>
public abstract class Bullet : MonoBehaviour
{
    [HideInInspector] public int damage;

    [SerializeField] protected float speed;
    protected Enemy target;


    /// <summary>
    /// Finds the bullet target
    /// </summary>
    /// <param name="_target"></param>
    public void FindTarget(Enemy _target)
    {
        target = _target;
    }

    protected virtual void Fire()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.transform.position - this.transform.position;

        //Checks if can move this frame or hit the target.
        //"speed * Time.deltaTime" makes sure that the bullet will be on the right place when hit the target
        //Making sure that the bullet will not overshoot the target
        if (dir.magnitude <= speed * Time.deltaTime)
        {
            HitTarget();
        }

        this.transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    }

    protected virtual void Update()
    {
        Fire();
    }

    protected virtual void HitTarget() { }
}
