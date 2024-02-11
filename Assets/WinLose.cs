using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages scene transitions based on game state changes.
/// </summary>

public class WinLose : MonoBehaviour
{
    private void Start() => GameManager.instance.OnGameStateChangedNotifier += CurrentStateBehaviour;

    private void OnDestroy() => GameManager.instance.OnGameStateChangedNotifier -= CurrentStateBehaviour;

    private void CurrentStateBehaviour(GameStates state)
    {
        if (state == GameStates.WinState)
        {
            SceneManager.LoadScene(1);
        }
        else if (state == GameStates.LoseState)
        {
            SceneManager.LoadScene(2);
        }
    }
}
