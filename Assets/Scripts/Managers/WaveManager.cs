using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages waves of enemies and their spawning during gameplay.
/// </summary>

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance { get; private set; }

    public UnityAction<List<GameObject>> OnEnemyListUpdated;

    public EnemyType type;
    [HideInInspector] public bool enemyStateActive;

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
        if (GameManager.instance != null)
            GameManager.instance.OnGameStateChangedNotifier += OnGameStateChange;

        amountOfEnemiesPerWave = maxEnemiesPerWave;
        UIManager.OnWaveChanged?.Invoke(currentWave, numberOfWaves);
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null) 
            GameManager.instance.OnGameStateChangedNotifier -= OnGameStateChange;
    }

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

    /// <summary>
    /// Spawns enemies based on the current wave's settings.
    /// </summary>
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
                if (go.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
                {
                    enemy.OnEnemyAttributesChanged?.Invoke(waveDifficulty);
                    Debug.Log("ehe");
                }
                enemies.Add(go);
                OnEnemyListUpdated?.Invoke(enemies);
                amountOfEnemiesPerWave--;
            }
        }
    }


    /// <summary>
    /// Timer for controlling the duration of each wave.
    /// </summary>
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

    /// <summary>
    /// Retrieves a random enemy prefab.
    /// </summary>
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