using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the overall game state and notifies subscribed components of state changes.
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public GameStates State;

    public UnityAction<GameStates> OnGameStateChangedNotifier;
    public UnityAction<GameStates> OnGameStateChanged;


    [SerializeField] private float durationInSeconds;
    [SerializeField] TimerHandler timerHandler;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);

        OnGameStateChanged += UpdateGameStates;
    }

    void OnDestroy() => OnGameStateChanged -= UpdateGameStates;

    private void Start() => UpdateGameStates(GameStates.BuildingState);

    public void UpdateGameStates(GameStates newState)
    {
        State = newState;
        switch (State)
        {
            case GameStates.BuildingState:
                BuildingStateHandler();
                break;
        }

        OnGameStateChangedNotifier?.Invoke(newState);
    }

    #region StatesUpdates
    void BuildingStateHandler() => timerHandler.OnTimerStarted?.Invoke(durationInSeconds);
    #endregion
}

public enum GameStates
{
    BuildingState,
    EnemyState,
    WinState,
    LoseState
}
