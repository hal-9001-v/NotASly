using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(EnvironmentInfo))]
[RequireComponent(typeof(SafeGroundChecker))]
[RequireComponent(typeof(Roper))]
[RequireComponent(typeof(Aligner))]
[RequireComponent(typeof(Venter))]
public class Player : MonoBehaviour
{
    [SerializeField] Transform cameraFollow;

    Mover mover => GetComponent<Mover>();
    Roper roper => GetComponent<Roper>();
    Piper piper => GetComponent<Piper>();
    Gripper gripper => GetComponent<Gripper>();
    Aligner aligner => GetComponent<Aligner>();
    Venter venter => GetComponent<Venter>();
    CharacterController characterController => GetComponent<CharacterController>();

    SafeGroundChecker safeGroundChecker => GetComponent<SafeGroundChecker>();

    CameraSelector cameraSelector => FindObjectOfType<CameraSelector>();

    [Header("Movement")]
    [SerializeField] float cameraSpeed = 5f;
    [SerializeField] float wallJumpSpeed = 20;

    [Header("Grip")]
    [SerializeField] Timer gripStayTimer;
    [SerializeField] Timer gripStartMoveTimer;
    [SerializeField] Timer gripCooldown;

    [SerializeField][Range(0, 1)] float wallJumpUpFactor = 0.8f;
    [SerializeField][Range(0, 1)] float movingWallJumpUpFactor = 0.8f;

    [Header("Vent")]
    float defaultControllerHeight;
    float defaultControllerRadius = 0.25f;

    [SerializeField][Range(0, 2)] float ventControllerHeight = 0.25f;
    [SerializeField][Range(0, 2)] float ventControllerRadius = 0.25f;


    Vector2 cameraDirection;

    [SerializeField] States currentState;

    Coroutine stunCoroutine;

    Vector3 direction;
    Vector2 input;
    enum States
    {
        Move,
        Fall,
        Rope,
        Pipe,
        Aligning,
        Stunned,
        Gripped,
        GrippedMoving,
        Venting,
    }

    private void Awake()
    {
        defaultControllerHeight = characterController.height;
        defaultControllerRadius = characterController.radius;
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

        var gripReady = gripCooldown.UpdateTimer();
        switch (currentState)
        {
            case States.Move:
                mover.Move(rotatedDirection);

                if (gripReady)
                {
                    if (mover.IsGrounded == false && gripper.Check())
                    {
                        ChangeState(States.Gripped);
                    }
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
            case States.Pipe:
                piper.RawMove(input);
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

                if (gripper.IsInWall)
                {
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
                }
                break;
            case States.GrippedMoving:
                if (gripper.MoveAlongWall() == false)
                {
                    ChangeState(States.Fall);
                };
                break;
            case States.Venting:
                venter.Move(cameraFollow.rotation * direction);

                if (venter.IsInEntry == false && venter.IsInside == false)
                {
                    ChangeState(States.Move);
                }
                break;
        }

    }

    private void LateUpdate()
    {
        cameraFollow.Rotate(Vector3.up, cameraDirection.x * cameraSpeed * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
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
            case States.Pipe:
                ChangeState(States.Move);
                mover.Jump();
                break;
            case States.Gripped:
                mover.Launch(Vector3.Lerp(gripper.GripNormal, Vector3.up, wallJumpUpFactor) * wallJumpSpeed);
                ChangeState(States.Move);
                break;
            case States.GrippedMoving:
                var moveDirection = gripper.MoveVelocity;
                moveDirection.y = 0;
                moveDirection.Normalize();

                var jumpDirection = Vector3.Lerp(gripper.GripNormal, moveDirection, 0.75f);

                mover.Launch(Vector3.Lerp(jumpDirection, Vector3.up, movingWallJumpUpFactor) * wallJumpSpeed);
                ChangeState(States.Move);

                break;

            case States.Venting:
                break;

        }
    }

    public void OnInteract()
    {
        if (roper.Check())
        {
            roper.Attach();
            Align(roper.RopePosition, States.Rope);
        }

        if (piper.Check())
        {
            piper.Attach();
            Align(piper.PipePosition, States.Pipe);
        }

        if (venter.Check())
        {
            ChangeState(States.Venting);
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
                piper.enabled = false;
                aligner.enabled = false;
                gripper.enabled = false;
                venter.enabled = false;

                gripCooldown.ResetTimer();

                characterController.height = defaultControllerHeight;
                characterController.radius = defaultControllerRadius;

                cameraSelector.UseFollowCamera();
                break;
            case States.Fall:
                mover.enabled = true;
                roper.enabled = false;
                piper.enabled = false;
                aligner.enabled = false;
                gripper.enabled = false;
                venter.enabled = false;

                characterController.height = defaultControllerHeight;
                characterController.radius = defaultControllerRadius;

                cameraSelector.UseFollowCamera();
                break;

            case States.Rope:
                mover.enabled = false;
                roper.enabled = true;
                piper.enabled = false;
                aligner.enabled = false;
                gripper.enabled = false;
                venter.enabled = false;

                characterController.height = defaultControllerHeight;
                characterController.radius = defaultControllerRadius;

                cameraSelector.UseFollowCamera();
                break;
            case States.Pipe:
                mover.enabled = false;
                roper.enabled = false;
                piper.enabled = true;
                aligner.enabled = false;
                gripper.enabled = false;
                venter.enabled = false;

                characterController.height = defaultControllerHeight;
                characterController.radius = defaultControllerRadius;

                cameraSelector.UseFollowCamera();
                break;
            case States.Stunned:
                mover.enabled = true;
                roper.enabled = false;
                piper.enabled = false;
                aligner.enabled = false;
                gripper.enabled = false;
                venter.enabled = false;

                characterController.height = defaultControllerHeight;
                characterController.radius = defaultControllerRadius;

                cameraSelector.UseFollowCamera();
                break;
            case States.Aligning:
                mover.enabled = false;
                roper.enabled = false;
                piper.enabled = false;
                aligner.enabled = true;
                gripper.enabled = false;
                venter.enabled = false;

                characterController.height = defaultControllerHeight;
                characterController.radius = defaultControllerRadius;

                cameraSelector.UseFollowCamera();
                break;

            case States.Gripped:
                mover.enabled = false;
                roper.enabled = false;
                piper.enabled = false;
                aligner.enabled = false;
                gripper.enabled = true;
                venter.enabled = false;

                gripStayTimer.ResetTimer();
                gripStartMoveTimer.ResetTimer();
                gripCooldown.ResetTimer();

                characterController.height = defaultControllerHeight;
                characterController.radius = defaultControllerRadius;

                cameraSelector.UseFollowCamera();
                break;
            case States.GrippedMoving:
                mover.enabled = false;
                roper.enabled = false;
                piper.enabled = false;
                aligner.enabled = false;
                gripper.enabled = true;
                venter.enabled = false;

                gripCooldown.ResetTimer();
                gripper.SetMoveDirection(cameraFollow.rotation * direction);

                characterController.height = defaultControllerHeight;
                characterController.radius = defaultControllerRadius;

                cameraSelector.UseFollowCamera();
                break;

            case States.Venting:
                mover.enabled = false;
                roper.enabled = false;
                piper.enabled = false;
                aligner.enabled = false;
                gripper.enabled = false;
                venter.enabled = true;

                characterController.height = ventControllerHeight;
                characterController.radius = ventControllerRadius;

                cameraSelector.UseVentCamera();
                break;
        }
        currentState = newState;
    }

    IEnumerator StunCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        ChangeState(States.Move);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ventControllerHeight);
    }
}
