using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Restart the game
/// </summary>

public class RestartHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            Debug.Log("The game has been restarted");
            SceneManager.LoadScene(0);
        }
    }
}
