using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OccupiedPositionsHandler : MonoBehaviour, IOccupiedPositions
{
    public static OccupiedPositionsHandler Instance;
    public HashSet<Vector3> occupiedPositions = new();
    public UnityAction<Vector3> OnOccupiedChanged;

    void Awake() { Instance = this; }

    public bool IsPositionOccupied(Vector3 pos)
    {
        return occupiedPositions.Contains(pos);
    }

    public void AddOccupiedPosition(Vector3 pos) => occupiedPositions.Add(pos);
}
