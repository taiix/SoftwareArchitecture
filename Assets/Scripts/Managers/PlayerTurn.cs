using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    private void Start() => GameManager.instance.OnGameStateChangedNotifier += GameStateChangeState;

    private void OnDestroy() => GameManager.instance.OnGameStateChangedNotifier -= GameStateChangeState;

    private void GameStateChangeState(GameStates state) => BuildingManager.Instance.PlayerInputEnabled(state == GameStates.BuildingState);
}
