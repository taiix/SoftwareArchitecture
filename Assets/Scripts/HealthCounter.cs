using UnityEngine;

/// <summary>
/// Handles tracking the number of enemies that have reached the goal and triggers events.
/// </summary>

public class HealthCounter : MonoBehaviour
{
    [SerializeField] private int enemiesReachedGoal;

    private void Start() => UIManager.OnHealthChanged?.Invoke(enemiesReachedGoal);

    private void Update()
    {
        if (enemiesReachedGoal > 0)
            return;
        GameManager.instance?.OnGameStateChanged?.Invoke(GameStates.LoseState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesReachedGoal > 0)
            {
                enemiesReachedGoal--;
                UIManager.OnHealthChanged?.Invoke(enemiesReachedGoal);
                Destroy(other.gameObject);
                WaveManager.instance?.enemies.Remove(other.gameObject);
            }
        }
    }
}
