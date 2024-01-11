using UnityEngine;

public class SingleTargetTower : Tower
{
    [SerializeField] private GameObject normal_bullet;

    public void Update()
    {
        if(normal_bullet == null) return;
        Attack(normal_bullet);
    }

    
}
