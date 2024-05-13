using System;
using UnityEngine;

[Serializable]
public class GoCheck : IAIBeahaviour
{
    AIMovement movement;

    [SerializeField] float speed = 5;
    [SerializeField] float inspectDistance = 1;

    [SerializeField] FSMTimeCondition inspectTimeCond;

    SoundSensor soundSensor;

    bool doneChecking;

    public FSMCondition DoneCondition => new FSMCondition(() => doneChecking);

    public FSMState GetFSMState(Guard body)
    {
        soundSensor = body.SoundSensor;
        movement = body.Movement;

        var move = new FSMState("GoCheck", StartMove, Move);

        var inspect = new FSMState("Inspect", Inspect);

        var doneInspecting = new FSMState("DoneInspecting", () => doneChecking = true);

        move.AddTransition(new FSMCondition(() =>
        {
            return Vector3.Distance(movement.transform.position, soundSensor.LastSensed.transform.position) < inspectDistance;
        }), inspect);

        return new FSMachine(move, false, "GoCheck", true, () => doneChecking = false);

    }

    void StartMove()
    {
        if(soundSensor.LastSensed == null)
            return;

        movement.Speed = speed;
        movement.GoToPosition(soundSensor.LastSensed.transform.position);
    }

    void Move()
    {

    }

    void Inspect()
    {
        movement.Stop();
    }


}
