using UnityEngine;

/// <summary>
/// Controls player turns based on the game state.
/// </summary>
public class PlayerTurn : MonoBehaviour
{
    private void Start() => GameManager.instance.OnGameStateChangedNotifier += GameStateChangeState;

    private void OnDestroy() => GameManager.instance.OnGameStateChangedNotifier -= GameStateChangeState;

    /// <summary>
    /// Enables or disables player input based on the game state.
    /// </summary>
    /// <param name="state"></param>
    private void GameStateChangeState(GameStates state) => BuildingManager.Instance.PlayerInputEnabled(state == GameStates.BuildingState);
}
