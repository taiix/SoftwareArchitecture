using System.Collections;
using UnityEngine;

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

    public void Execute()
    {
        enemy.speed = slowSpeed;

        enemy.StartCoroutine(UndoAfterDuration(duration));
    }

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
