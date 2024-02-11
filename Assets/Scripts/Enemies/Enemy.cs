using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Abstract class representing an enemy unit in the game.
/// </summary>
public abstract class Enemy : MonoBehaviour, IEnemy
{
    public static UnityAction OnObjectCreated;

    public float speed;

    public int gold;

    [SerializeField] protected List<Vector3> path;
    [SerializeField] protected int currentWaypoint;

    /// <summary>
    /// Moves the enemy along the predefined path.
    /// </summary>
    public void Moving()
    {
        if (path != null && currentWaypoint < path.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[currentWaypoint], speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, path[currentWaypoint]) < 0.1f)
            {
                currentWaypoint++;
            }
        }
    }

    protected virtual void Start()
    {
        RandomPathGenerator.OnPathCreated += HandlePath;

        OnObjectCreated?.Invoke();
    }

    protected virtual void OnDestroy() => RandomPathGenerator.OnPathCreated -= HandlePath;

    /// <summary>
    /// Handles the received path data.
    /// </summary>
    /// <param name="recievedPath"></param>
    protected virtual void HandlePath(List<Vector3> recievedPath) => this.path = recievedPath;
}
