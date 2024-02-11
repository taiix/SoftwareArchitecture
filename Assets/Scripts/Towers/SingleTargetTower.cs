using UnityEngine;

/// <summary>
/// Class representing a single-target tower in the game.
/// </summary>
public class SingleTargetTower : Tower
{
    [SerializeField] private GameObject normal_bullet;

    public void Update()
    {
        if(normal_bullet == null) return;
        Attack(normal_bullet);
    }
}
