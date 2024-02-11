using System.Collections;
using UnityEngine;

/// <summary>
/// Command pattern implementation for applying a slow debuff to an enemy in the game.
/// </summary>
public class SlowDebuffCommand : ICommand
{
    private Enemy enemy;

    private float originalSpeed;
    private float slowSpeed;
    private float duration;

    public SlowDebuffCommand(Enemy enemy, float originalSpeed, float slowSpeed, float duration)
    {
        this.enemy = enemy;
        this.originalSpeed = originalSpeed;
        this.slowSpeed = slowSpeed;
        this.duration = duration;
    }

    /// <summary>
    /// Executes the slow debuff command by reducing the enemy's speed and it's restoring it after some time
    /// </summary>
    public void Execute()
    {
        enemy.speed = slowSpeed;

        enemy.StartCoroutine(UndoAfterDuration(duration));
    }

    /// <summary>
    /// Undoes the slow debuff by restoring the enemy's original speed.
    /// </summary>
    public void Undo()
    {
        enemy.speed = originalSpeed;
    }

    IEnumerator UndoAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        Undo();
    }
}
