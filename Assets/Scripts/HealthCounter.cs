using UnityEngine;
using UnityEngine.Events;

public class HealthCounter : MonoBehaviour
{
    public static UnityAction OnHitDetected;

    [SerializeField] private int enemiesReachedGoal;

    private void Start() => UIManager.OnHealthChanged?.Invoke(enemiesReachedGoal);

    private void Update()
    {
        if (enemiesReachedGoal > 0)
            return;
        GameManager.instance.OnGameStateChanged?.Invoke(GameStates.LoseState);
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
                WaveManager.instance.enemies.Remove(other.gameObject);
            }
        }
    }
}
