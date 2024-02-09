using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject indicator;

    [SerializeField] private Grid grid;

    private void Start() => InputManager.Instance.OnMousePositionChange += PositionChangeListener;

    private void OnDestroy() => InputManager.Instance.OnMousePositionChange -= PositionChangeListener;

    private void PositionChangeListener(Vector3 pos)
    {
        if (grid == null) return;

        Vector3Int gridPosition = grid.WorldToCell(pos);
        indicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
