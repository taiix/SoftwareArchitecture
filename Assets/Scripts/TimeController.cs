using UnityEngine;

/// <summary>
/// Controls the time scale based on mouse scroll input.
/// </summary>

public class TimeController : MonoBehaviour
{
    private void Update()
    {
        TimeControlling();
    }

    private void TimeControlling()
    {
        float scrollSpeed = Input.mouseScrollDelta.y;
        if (scrollSpeed != 0)
        {
            float newScale = Time.timeScale + scrollSpeed * 0.03f;
            Time.timeScale = Mathf.Clamp(newScale, 0, 1);
        }
    }
}
