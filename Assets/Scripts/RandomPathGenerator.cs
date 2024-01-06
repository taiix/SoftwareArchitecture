using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Some ideas and optimizations from AI
/// </summary>

public class RandomPathGenerator : MonoBehaviour
{
    public static UnityAction<List<Vector3>> OnPathCreated;

    [SerializeField] GameObject waypointPrefab;

    [SerializeField] Transform startWaypoint;
    [SerializeField] Transform endWaypoint;

    [SerializeField] int maxWaypoints;

    [SerializeField] Grid grid;

    private const float _PositionY = 0.01f;
    private const float diff = 0.1f;

    /// <summary>
    /// closeDistMultiplier = 1 means the path will go directly towards the end waypoint
    /// closeDistMultiplier = 0.5f means the path will be random for 1/2 of the way
    /// closeDistMultiplier = 0.25f means the path will be random for 1/4 of the way
    /// </summary>
    [Range(0,1)]
    [SerializeField] private float closeDistMultiplier = 0.25f;      //How correct the path is. 0.25f is 1/4 of the distance between start and end.

    /// <summary>
    /// How many waypoints to create based on the distance between start waypoint and end waypoint
    /// For instance, dist = 5, optimalDistMultiplier = 2; that means dist * multiplier = 10. There should be at least 10 waypoints in the path to the end
    /// </summary>
    [Range(0, 3)]
    [SerializeField]private float optimalDistMultiplier = 2f;

    private Vector3[] dirs = new Vector3[] {
        new Vector3(-1, _PositionY, 0),
        new Vector3(1, _PositionY, 0),
        new Vector3(0, _PositionY, 1),
        new Vector3(0, _PositionY, -1) };

    [SerializeField]private List<Vector3> visited = new();

    private List<int> randomSequence = new() { 0, 1, 2, 3 };

    [SerializeField] private OccupiedPositionsHandler occupiedPos;

#if UNITY_EDITOR
    private void Awake()
    {
        startWaypoint.position = new Vector3(startWaypoint.position.x, _PositionY, startWaypoint.position.z);
        endWaypoint.position = new Vector3(endWaypoint.position.x, _PositionY, endWaypoint.position.z);

        if (IsNotRounded(startWaypoint.position) || IsNotRounded(endWaypoint.position))
        {
            Debug.LogError("Position.x and position.z must be rounded to 0.5f");
            StopGameInEditor();
        }

        if (!IsPositionWithinGrid(startWaypoint.position) || !IsPositionWithinGrid(endWaypoint.position))
        {
            Debug.LogError($"The position of start waypoint and end waypoint should be inside the grid");
            StopGameInEditor();
        }
    }

    private void StopGameInEditor() => UnityEditor.EditorApplication.isPlaying = false;
#endif

    private void Start()
    {
        startWaypoint.position = new Vector3(startWaypoint.position.x, _PositionY, startWaypoint.position.z);
        endWaypoint.position = new Vector3(endWaypoint.position.x, _PositionY, endWaypoint.position.z);

        Enemy.OnObjectCreated += SendPath;

        Generate();
    }

    private void OnDestroy() => Enemy.OnObjectCreated -= SendPath;

    private void SendPath() => OnPathCreated?.Invoke(visited);

    void Generate()
    {
        visited.Clear();

        Vector3 currentWaypoint = startWaypoint.position;
        visited.Add(currentWaypoint);

        int minDist = (int)Vector3.Distance(startWaypoint.position, endWaypoint.position);
        int closeDist = (int)(minDist * closeDistMultiplier);
        int optimalDist = (int)(minDist * optimalDistMultiplier);

        for (int i = 0; i < maxWaypoints; i++)
        {
            Vector3 closestVectorToTheEnd = Vector3.zero;
            float closestDistanceToTheEnd = float.MaxValue;

            int rand = Random.Range(0, dirs.Length);
            int randomInt = 0;

            if (Vector3.Distance(currentWaypoint, endWaypoint.position) > closeDist)
            {
                #region RandomPath
                for (int j = 0; j < dirs.Length; j++)
                {
                    randomInt = Random.Range(0, randomSequence.Count);

                    randomSequence.RemoveAt(randomInt);
                    Vector3 newPos = currentWaypoint + dirs[randomInt];

                    if (!visited.Contains(newPos))
                    {
                        float dist = Vector3.Distance(newPos, endWaypoint.position);

                        if (dist < closestDistanceToTheEnd)
                        {
                            closestDistanceToTheEnd = dist;
                            closestVectorToTheEnd = dirs[randomInt]; //Random direction, not closest in the case
                        }
                    }
                }

                randomSequence.Clear();
                for (int q = 0; q < 4; q++)
                {
                    randomSequence.Add(q);
                }
                #endregion
            }
            else
            {
                #region ClosestPath
                foreach (Vector3 direction in dirs)
                {
                    Vector3 newPos = currentWaypoint + direction;

                    if (!visited.Contains(newPos))
                    {
                        float dist = Vector3.Distance(newPos, endWaypoint.position);

                        if (dist < closestDistanceToTheEnd)
                        {
                            closestDistanceToTheEnd = dist;
                            closestVectorToTheEnd = direction; //Closest direction
                        }
                    }
                }
                #endregion
            }

            if (closestVectorToTheEnd != Vector3.zero)
            {
                Vector3 newPos = currentWaypoint + closestVectorToTheEnd;
                if (currentWaypoint != endWaypoint.position && !IsPositionInList(newPos, visited) && IsPositionWithinGrid(newPos))
                {
                    if (Neighbours(newPos))
                    {
                        currentWaypoint = new Vector3(newPos.x, _PositionY, newPos.z);
                        visited.Add(currentWaypoint);
                    }
                }
            }
        }
        if (visited.Count >= optimalDist && currentWaypoint == endWaypoint.position)
        {
            GeneratePath(visited);
        }
        else
        {
            Generate();
        }
    }

    void GeneratePath(List<Vector3> path)
    {
        foreach (Vector3 newPos in path)
        {
            GameObject go = Instantiate(waypointPrefab, new Vector3(newPos.x, _PositionY, newPos.z), Quaternion.identity);
            occupiedPos.AddOccupiedPosition(go.transform.position);
        }
    }

    bool Neighbours(Vector3 currentWaypoint)
    {
        int nbCount = 0;
        foreach (Vector3 dir in dirs)
        {
            Vector3 newPos = currentWaypoint + dir;
            if (IsPositionInList(newPos, visited))
            {
                nbCount++;
            }
        }

        if (nbCount < 2)
        {
            return true;
        }
        else { return false; }
    }

    bool IsPositionInList(Vector3 position, List<Vector3> list)
    {
        foreach (Vector3 pos in list)
        {
            if (Vector3.Distance(position, pos) < diff)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsNotRounded(Vector3 position)
    {
        return Mathf.Approximately(position.x, Mathf.Round(position.x)) || Mathf.Approximately(position.z, Mathf.Round(position.z));
    }

    private bool IsPositionWithinGrid(Vector3 position)
    {
        return Physics.Raycast(position, Vector3.down, out RaycastHit hit, Mathf.Infinity);
    }
}