using UnityEngine;

public interface IOccupiedPositions
{
    bool IsPositionOccupied(Vector3 pos);
    void AddOccupiedPosition(Vector3 pos);
}
