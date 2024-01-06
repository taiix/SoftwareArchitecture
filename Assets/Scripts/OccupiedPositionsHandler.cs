using System.Collections.Generic;
using UnityEngine;

public class OccupiedPositionsHandler : MonoBehaviour, IOccupiedPositions
{
    public HashSet<Vector3> occupiedPositions = new();

    public bool IsPositionOccupied(Vector3 pos)
    {
        return occupiedPositions.Contains(pos);
    }

    public void AddOccupiedPosition(Vector3 pos) => occupiedPositions.Add(pos);

}
