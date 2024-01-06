using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour
{
    public static UnityAction OnObjectCreated;

    public UnityAction<int> OnEnemyAttributesChanged;

    [SerializeField] protected float health;
    [SerializeField] protected float gold;
    [SerializeField] protected float speed;

    [SerializeField] protected List<Vector3> path;
    [SerializeField] protected int currentWaypoint;

    protected void Moving()
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

    private void Awake() => OnEnemyAttributesChanged += DifficultyChange;
    

    protected virtual void Start()
    {
        RandomPathGenerator.OnPathCreated += HandlePath;

        OnObjectCreated?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        RandomPathGenerator.OnPathCreated -= HandlePath;
        OnEnemyAttributesChanged -= DifficultyChange;
    }

    protected virtual void HandlePath(List<Vector3> recievedPath) => this.path = recievedPath;

    private void DifficultyChange(int increaseAttribute) => health += increaseAttribute;

}
