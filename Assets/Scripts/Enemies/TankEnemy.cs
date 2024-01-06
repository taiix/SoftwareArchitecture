using UnityEngine;

public class TankEnemy : Enemy
{
    [SerializeField] private float maxMovingTimeInSeconds;
    [SerializeField] private float maxIdleTimeInSeconds;

    private float currentSpeed;

    private float movingTimer = 0;
    private float idleTimer = 0;

    private TankEnemyState state;

    protected override void Start()
    {
        base.Start();
        currentSpeed = speed;
    }

    private void Update()
    {
        TimeManagement();
        StateManager();
    }

    void StateManager()
    {
        switch (state)
        {
            case TankEnemyState.MoveState:
                idleTimer = 0;
                speed = currentSpeed;
                Moving();
                break;
            case TankEnemyState.IdleState:

                speed = 0;
                break;
        }
    }

    private void TimeManagement()
    {
        switch (state)
        {
            case TankEnemyState.MoveState:
                movingTimer += Time.deltaTime;
                if (movingTimer >= maxMovingTimeInSeconds)
                {
                    state = TankEnemyState.IdleState;
                    movingTimer = 0;
                }
                break;
            case TankEnemyState.IdleState:
                idleTimer += Time.deltaTime;
                if (idleTimer >= maxIdleTimeInSeconds)
                {
                    state = TankEnemyState.MoveState;
                    idleTimer = 0;
                }
                break;
        }
    }
}

public enum TankEnemyState
{
    MoveState,
    IdleState
}