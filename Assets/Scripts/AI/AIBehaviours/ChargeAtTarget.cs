using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChargeAtTarget : IAIBeahaviour
{

    [SerializeField] float chargeSpeed;
    [SerializeField] HurtBox hurtBox;
    [SerializeField] FSMTimeCondition faceTimeCond;
    [SerializeField] FSMTimeCondition recoverTimeCond;

    TargetProvider targetProvider;

    Vector3 direction;

    Transform target;
    AIMovement movement;

    bool doneCharging;

    public FSMCondition DoneCondition => new FSMCondition(() => doneCharging);

    public FSMState GetFSMState(Guard guard)
    {
        movement = guard.GetComponent<AIMovement>();
        targetProvider = guard.TargetProvider;

        var idle = new FSMState("Idle", StartIdle, FaceTarget);

        var charge = new FSMState("Charge", StartCharge, Charge);

        var recover = new FSMState("Recover", Recover, null, () => doneCharging = true);

        idle.AddTransition(charge, faceTimeCond);
        charge.AddTransition(recover, new FSMCondition(() => guard.WallSensor.IsTouchingWall));
        recover.AddTransition(idle, recoverTimeCond);

        return new FSMachine(idle, false, "Charge", true, () => doneCharging = false);
    }

    void StartCharge()
    {
        if (target == null)
        {
            target = targetProvider.LastTarget;
        }

        if (target == null)
            return;

        direction = target.position - movement.transform.position;
        direction.y = 0;
        direction.Normalize();

        doneCharging = false;
    }

    void Charge()
    {
        movement.Speed = chargeSpeed;
        movement.RawMovement(direction);
        hurtBox.Apply = true;
    }


    void StartIdle()
    {
        if (targetProvider.HasTargets)
        {
            target = targetProvider.UpdateTarget();
        }
        else if (target == null)
        {
            return;
        }

        hurtBox.Apply = false;
        movement.Stop();
    }

    void FaceTarget()
    {
        if (target == null)
            return;

        movement.LookAt(target.position);
    }

    void Recover()
    {
        hurtBox.Apply = false;
        movement.Stop();
    }

}
