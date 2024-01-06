using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            case GameStates.WinState:
                WinStateHandler();
                break;
            case GameStates.LoseState:
                LoseStateHandler();
                break;
        }

        OnGameStateChangedNotifier?.Invoke(newState);
    }

    #region StatesUpdates
    void BuildingStateHandler() => timerHandler.OnTimerStarted?.Invoke(durationInSeconds);

    void WinStateHandler()
    {
        Debug.Log("win win win");
    }

    void LoseStateHandler() { }
    #endregion

}

public enum GameStates
{
    BuildingState,
    EnemyState,
    WinState,
    LoseState
}
