using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(EnvironmentInfo))]
[RequireComponent(typeof(SafeGroundChecker))]
[RequireComponent(typeof(Roper))]
[RequireComponent(typeof(Aligner))]
public class Player : MonoBehaviour
{
    [SerializeField] Transform cameraFollow;

    Mover mover => GetComponent<Mover>();
    Roper roper => GetComponent<Roper>();
    Gripper gripper => GetComponent<Gripper>();
    Aligner aligner => GetComponent<Aligner>();

    SafeGroundChecker safeGroundChecker => GetComponent<SafeGroundChecker>();

    [Header("Movement")]
    [SerializeField] float cameraSpeed = 5f;
    [SerializeField] float wallJumpSpeed = 20;

    [Header("Grip")]
    [SerializeField] Timer gripStayTimer;
    [SerializeField] Timer gripStartMoveTimer;

    [SerializeField][Range(0, 1)] float wallJumpUpFactor = 0.8f;


    Vector2 cameraDirection;

    States currentState;

    Coroutine stunCoroutine;

    Vector3 direction;

    enum States
    {
        Move,
        Fall,
        Rope,
        Aligning,
        Stunned,
        Gripped,
        GrippedMoving,
    }

    private void Awake()
    {
        ChangeState(States.Move);
    }

    private void Start()
    {
        var environmentInfo = GetComponent<EnvironmentInfo>();
        environmentInfo.OnFallAction += () =>
        {
            Stun(1f);
            mover.TeleportTo(safeGroundChecker.SafePosition);
        };
    }

    private void Update()
    {
        var rotatedDirection = cameraFollow.rotation * direction;


        switch (currentState)
        {
            case States.Move:
                mover.Move(rotatedDirection);
                if (gripper.Check())
                {
                    ChangeState(States.Gripped);
                }
                break;

            case States.Fall:
                if (mover.IsGrounded)
                {
                    ChangeState(States.Move);
                }
                else
                {
                    mover.Move(Vector3.zero);
                }
                break;
            case States.Rope:
                roper.Move(rotatedDirection);
                break;
            case States.Aligning:

                break;
            case States.Stunned:
                break;
            case States.Gripped:
                if (gripStayTimer.UpdateTimer())
                {
                    ChangeState(States.Fall);
                }

                if (direction.magnitude > 0.25f)
                {
                    if (gripStartMoveTimer.UpdateTimer())
                    {
                        ChangeState(States.GrippedMoving);
                    }
                }
                else
                {
                    gripStartMoveTimer.ResetTimer();
                }
                break;
            case States.GrippedMoving:
                if (gripper.MoveAlongWall() == false)
                {
                    ChangeState(States.Fall);
                };
                break;
        }

    }

    private void LateUpdate()
    {
        cameraFollow.Rotate(Vector3.up, cameraDirection.x * cameraSpeed * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        var input = value.Get<Vector2>();
        direction = new Vector3(input.x, 0, input.y);
    }

    public void OnCamera(InputValue value)
    {
        cameraDirection = value.Get<Vector2>();
    }

    public void OnJump()
    {
        switch (currentState)
        {
            case States.Move:
                if (mover.IsGrounded)
                    mover.Jump();
                break;
            case States.Rope:
                ChangeState(States.Move);
                mover.Jump();
                break;
            case States.Gripped:
            case States.GrippedMoving:
                var normal = gripper.GripNormal;

                mover.enabled = true;
                mover.Launch(Vector3.Lerp(normal, Vector3.up, wallJumpUpFactor) * wallJumpSpeed);
                ChangeState(States.Move);
                break;

        }
    }

    public void OnInteract()
    {
        if (roper.Check())
        {
            Align(roper.RopePosition, States.Rope);
        }
    }

    void Align(Vector3 position, States nextState)
    {
        ChangeState(States.Aligning);
        aligner.Align(position, () =>
        {
            ChangeState(nextState);
        });
    }

    public void Stun(float time)
    {
        if (stunCoroutine != null)
            StopCoroutine(stunCoroutine);

        ChangeState(States.Stunned);
        stunCoroutine = StartCoroutine(StunCoroutine(time));
    }

    void ChangeState(States newState)
    {
        switch (newState)
        {
            case States.Move:
                mover.enabled = true;
                roper.enabled = false;
                aligner.enabled = false;
                gripper.enabled = false;
                break;
            case States.Fall:
                mover.enabled = true;
                roper.enabled = false;
                aligner.enabled = false;
                gripper.enabled = false;
                break;

            case States.Rope:
                mover.enabled = false;
                roper.enabled = true;
                aligner.enabled = false;
                gripper.enabled = false;
                break;
            case States.Stunned:
                mover.enabled = true;
                roper.enabled = false;
                aligner.enabled = false;
                gripper.enabled = false;
                break;
            case States.Aligning:
                mover.enabled = false;
                roper.enabled = false;
                aligner.enabled = true;
                gripper.enabled = false;
                break;

            case States.Gripped:
                mover.enabled = false;
                roper.enabled = false;
                aligner.enabled = false;
                gripper.enabled = true;

                gripStayTimer.ResetTimer();
                gripStartMoveTimer.ResetTimer();
                break;
            case States.GrippedMoving:
                mover.enabled = false;
                roper.enabled = false;
                aligner.enabled = false;
                gripper.enabled = true;

                gripper.SetMoveDirection(cameraFollow.rotation * direction);
                break;
        }
        currentState = newState;
    }

    IEnumerator StunCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        ChangeState(States.Move);
    }

}
