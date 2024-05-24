using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour, IRespawnable
{
    [SerializeField] SightSensor sightSensor;
    public SightSensor SightSensor => sightSensor;

    [SerializeField] WallSensor wallSensor;
    public WallSensor WallSensor => wallSensor;

    [SerializeField] TouchSensor touchSensor;
    public TouchSensor TouchSensor => touchSensor;

    [SerializeField] SoundSensor soundSensor;
    public SoundSensor SoundSensor => soundSensor;

    public AIMovement Movement => GetComponent<AIMovement>();

    public TargetProvider TargetProvider { get; private set; }

    [SerializeField] PatrolAround patrolAround;
    [SerializeField] ChargeAtTarget chargeAtTarget;
    [SerializeField] GoCheck goCheck;

    FSMachine machine;

    private void Awake()
    {
        TargetProvider = new TargetProvider();

        sightSensor.OnSense += (s) =>
        {
            TargetProvider.AddTarget(s.transform);
        };

        sightSensor.OnLost += (s) =>
        {
            TargetProvider.RemoveTarget(s.transform);
        };

        touchSensor.OnSense += (s) =>
        {
            TargetProvider.AddTarget(s.transform);
        };

        touchSensor.OnLost += (s) =>
        {
            TargetProvider.RemoveTarget(s.transform);
        };

    }

    void Start()
    {
        CreateMachine();
    }

    void CreateMachine()
    {
        FSMState chaseState = new FSMState("Chase", StartChase, Chase);

        var patrolState = patrolAround.GetFSMState(this);
        var chargeState = chargeAtTarget.GetFSMState(this);
        var goCheckState = goCheck.GetFSMState(this);

        FSMCondition foundPlayer = new FSMCondition(() => TargetProvider.HasTargets);

        patrolState.AddTransition(chaseState, foundPlayer);
        patrolState.AddTransition(goCheckState, new FSMCondition(() => soundSensor.IsSensing));

        chaseState.AddTransition(new FSMCondition(() =>
        {
            var target = TargetProvider.LastTarget;

            if (target == null)
            {
                return false;
            }

            return Vector3.Distance(transform.position, target.position) < 5;
        }), chargeState);

        goCheckState.AddTransition(patrolState, goCheck.DoneCondition);
        goCheckState.AddTransition(chaseState, foundPlayer);

        machine = new FSMachine(patrolState, true, "Guard");
    }

    void StartChase()
    {
        TargetProvider.UpdateTarget();
    }

    void Chase()
    {
        if (TargetProvider.HasTargets == false)
            return;

        Movement.GoToPosition(TargetProvider.LastTarget.position);
    }


    void Update()
    {
        machine.Update();
    }

    public void Respawn()
    {
        machine.Reset();
    }
}
