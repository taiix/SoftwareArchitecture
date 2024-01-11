using UnityEngine;

public class Debuff_Tower : Tower
{
    [SerializeField] private GameObject debuff_bullet;

    public void Update()
    {
        if (debuff_bullet == null) return;
        Attack(debuff_bullet);
    }
}
