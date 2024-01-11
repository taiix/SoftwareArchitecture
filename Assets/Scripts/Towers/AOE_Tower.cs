using UnityEngine;

public class AOE_Tower : Tower
{
    [SerializeField] private GameObject AOE_bullet;

    public void Update()
    {
        if (AOE_bullet == null) return;
        Attack(AOE_bullet);
    }
}
