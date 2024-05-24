using System;
using UnityEngine;

[Serializable]
public class PatrolAround : IAIBeahaviour
{

    [SerializeField] AIPath path;
    public AIPath Path => path;
    [SerializeField] float speed;

    [SerializeField] FSMTimeCondition idleTimeCond;

    int currentPoint;

    AIMovement movement;

    public FSMCondition DoneCondition => throw new NotImplementedException();
    public FSMCondition NoPathCondition => new FSMCondition(() => path == null);

    public FSMState GetFSMState(Guard guard)
    {
        movement = guard.GetComponent<AIMovement>();

        if (movement == null)
        {
            Debug.LogError("No AIMovement component found on body");
            return null;
        }

        var idle = new FSMState("Idle", movement.Stop);

        if (path != null)
        {
            var move = new FSMState("Move", NextPoint, MoveToPoint);
            var closeToPointCond = new FSMCondition(() => path.IsInsideControlPoint(currentPoint, guard.transform.position));
            move.AddTransition(idle, closeToPointCond);

            idle.AddTransition(move, idleTimeCond);
            currentPoint = path.GetClosestControlPointIndex(guard.transform.position);
        }

        return new FSMachine(idle, true);
    }


    void MoveToPoint()
    {
        if (movement == null)
        {
            Debug.LogError("No AIMovement component found on body");
            return;
        }

        if (path == null)
        {
            return;
        }

        movement.Speed = speed;
        movement.GoToPosition(path.GetControlPoint(currentPoint));
    }

    void NextPoint()
    {
        if (path == null)
        {
            return;
        }

        currentPoint = path.NextPoint(currentPoint);
    }

}
