using UnityEngine;

/// <summary>
/// Class representing an AOE tower in the game.
/// </summary>
public class AOE_Tower : Tower
{
    [SerializeField] private GameObject AOE_bullet;

    public void Update()
    {
        if (AOE_bullet == null) return;
        Attack(AOE_bullet);
    }
}
