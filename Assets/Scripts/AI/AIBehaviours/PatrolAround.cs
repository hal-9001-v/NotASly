using System;
using UnityEngine;

[Serializable]
public class PatrolAround : IAIBeahaviour
{

    [SerializeField] AIPath path;
    [SerializeField] float speed;

    [SerializeField] FSMTimeCondition idleTimeCond;

    int currentPoint;

    AIMovement movement;

    public FSMCondition DoneCondition => throw new NotImplementedException();

    public FSMState GetFSMState(Guard guard)
    {
        movement = guard.GetComponent<AIMovement>();

        if (movement == null)
        {
            Debug.LogError("No AIMovement component found on body");
            return null;
        }

        var idle = new FSMState("Idle", movement.Stop);

        var move = new FSMState("Move", NextPoint, MoveToPoint);

        var closeToPointCond = new FSMCondition(() => path.IsInsideControlPoint(currentPoint, guard.transform.position));

        move.AddTransition(idle, closeToPointCond);
        idle.AddTransition(move, idleTimeCond);

        currentPoint = path.GetClosestControlPointIndex(guard.transform.position);

        return new FSMachine(move, true);

    }


    void MoveToPoint()
    {
        if (movement == null)
        {
            Debug.LogError("No AIMovement component found on body");
            return;
        }
        movement.Speed = speed;
        movement.GoToPosition(path.GetControlPoint(currentPoint));
    }

    void NextPoint()
    {
        currentPoint = path.NextPoint(currentPoint);
    }

}
