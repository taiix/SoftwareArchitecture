using UnityEngine;

/// <summary>
/// Manages the grid and indicator placement based on mouse position.
/// </summary>
public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject indicator;

    [SerializeField] private Grid grid;

    private void Start() => InputManager.Instance.OnMousePositionChange += PositionChangeListener;

    private void OnDestroy() => InputManager.Instance.OnMousePositionChange -= PositionChangeListener;

    /// <summary>
    /// Updates the position of the indicator based on the mouse position change.
    /// </summary>
    /// <param name="pos"></param>
    private void PositionChangeListener(Vector3 pos)
    {
        if (grid == null || indicator == null) return;

        Vector3Int gridPosition = grid.WorldToCell(pos);
        indicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
