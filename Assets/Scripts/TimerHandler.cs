using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles timer functionality and triggers events when the timer starts.
/// </summary>
public class TimerHandler : MonoBehaviour
{
    public UnityAction<float> OnTimerStarted;

    private void OnEnable() => OnTimerStarted += StartTimer;

    public void StartTimer(float duration) {
        StartCoroutine(BuildingStartTimer(duration));
    }

    public IEnumerator BuildingStartTimer(float duration)
    {
        float timer = duration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            UIManager.OnTimeChanged?.Invoke(timer);
            yield return null;
        }
        GameManager.instance.OnGameStateChanged?.Invoke(GameStates.EnemyState);
    }
}
