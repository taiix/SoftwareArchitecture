using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    /*
    • The game has at least 5 waves of enemies, each wave is more difficult than the previous one.
    • Properties of a wave can be configured in the Unity Editor without changing the code, e.g. one or multiple of: 
      enemy types, enemy amounts, enemy combinations, delay between enemies, percentage of each enemy type, chance of spawning, etc.
    • In between waves the players have a short building phase to sell/destroy, build and upgrade the towers.
    */
    public static WaveManager instance { get; private set; }

    public UnityAction<List<GameObject>> OnEnemyListUpdated;

    public bool enemyStateActive;
    public EnemyType type;

    public int waveDifficulty;
    [HideInInspector] public List<GameObject> enemies = new();

    [Range(5, 10)]
    [SerializeField] private int numberOfWaves;
    [SerializeField] private int maxEnemiesPerWave;
    [SerializeField] private float spawningDelay;
    [SerializeField] private float waveDuration;
    [SerializeField] private Transform spawnPos;

    [SerializeField] private GameObject normalEnemyPrefab;
    [SerializeField] private GameObject fastEnemyPrefab;
    [SerializeField] private GameObject tankEnemyPrefab;

    private int currentWave = 1;

    private float waveTimer;
    [SerializeField] private int amountOfEnemiesPerWave;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
    }

    private void Start()
    {
        GameManager.instance.OnGameStateChangedNotifier += OnGameStateChange;
        amountOfEnemiesPerWave = maxEnemiesPerWave;
        UIManager.OnWaveChanged?.Invoke(currentWave, numberOfWaves);
    }

    private void OnDestroy() => GameManager.instance.OnGameStateChangedNotifier -= OnGameStateChange;

    private void OnGameStateChange(GameStates state)
    {
        enemyStateActive = state == GameStates.EnemyState;
        if (enemyStateActive) StartCoroutine(WaveStartTimer(waveDuration));
    }

    private void Update()
    {
        WaveCounter();
        if (!enemyStateActive) return;

        if (Input.GetKeyDown(KeyCode.Space)) enemies.Clear();
        SpawningEnemies();
    }

    private void SpawningEnemies()
    {
        if (amountOfEnemiesPerWave <= 0 && enemies.Count == 0)
        {
            if (GameManager.instance?.State == GameStates.EnemyState)
            {
                GameManager.instance?.OnGameStateChanged?.Invoke(GameStates.BuildingState);

                currentWave++;
                UIManager.OnWaveChanged?.Invoke(currentWave, numberOfWaves);
                waveDifficulty++;
                amountOfEnemiesPerWave = maxEnemiesPerWave;
            }
            return;
        }
        else if (amountOfEnemiesPerWave > 0 && waveTimer % spawningDelay < Time.deltaTime)
        {
            GameObject enemyPrefab = GetEnemyPrefab();
            if (enemyPrefab != null)
            {
                GameObject go = Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity);
                if (go.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.OnEnemyAttributesChanged?.Invoke(waveDifficulty);
                }
                enemies.Add(go);
                OnEnemyListUpdated?.Invoke(enemies);
                amountOfEnemiesPerWave--;
            }
        }
    }

    IEnumerator WaveStartTimer(float duration)
    {
        float timer = 0;
        waveTimer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            this.waveTimer = timer;
            yield return null;
        }
    }

    //fix this
    private GameObject GetEnemyPrefab()
    {
        var enemyTypeValues = Enum.GetValues(typeof(EnemyType));
        EnemyType getRandomType = (EnemyType)enemyTypeValues.GetValue(UnityEngine.Random.Range(0, enemyTypeValues.Length));

        GameObject enemyPrefab = null;
        switch (getRandomType)
        {
            case EnemyType.NormalEnemy:
                enemyPrefab = normalEnemyPrefab;
                break;
            case EnemyType.FastEnemy:
                enemyPrefab = fastEnemyPrefab;
                break;
            case EnemyType.TankEnemy:
                enemyPrefab = tankEnemyPrefab;
                break;
        }
        return enemyPrefab;
    }

    void WaveCounter()
    {
        if (currentWave == numberOfWaves)
        {
            GameManager.instance.OnGameStateChanged?.Invoke(GameStates.WinState);
            currentWave = 0;
        }
    }
}

public enum EnemyType
{
    NormalEnemy,
    FastEnemy,
    TankEnemy
}