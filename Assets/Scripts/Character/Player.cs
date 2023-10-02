using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(EnvironmentInfo))]
[RequireComponent(typeof(SafeGroundChecker))]
[RequireComponent(typeof(Roper))]
[RequireComponent(typeof(Aligner))]
[RequireComponent(typeof(Venter))]
public class Player : MonoBehaviour
{
    [SerializeField] Transform cameraFollow;

    Health Health => GetComponent<Health>();
    Mover Mover => GetComponent<Mover>();
    Meleer Meleer => GetComponent<Meleer>();
    Roper Roper => GetComponent<Roper>();
    Piper Piper => GetComponent<Piper>();
    FallRoper FallRoper => GetComponent<FallRoper>();
    WallSticker WallSticker => GetComponent<WallSticker>();
    Gripper Gripper => GetComponent<Gripper>();
    Aligner Aligner => GetComponent<Aligner>();
    Venter Venter => GetComponent<Venter>();

    CharacterController CharacterController => GetComponent<CharacterController>();
    SafeGroundChecker SafeGroundChecker => GetComponent<SafeGroundChecker>();
    CameraSelector CameraSelector => FindObjectOfType<CameraSelector>();
    PlayerHud PlayerHud => FindObjectOfType<PlayerHud>();

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

    bool interacting;

    IPathFollower currentPathFollower;
    enum States
    {
        Move,
        Fall,
        Path,
        Aligning,
        Stunned,
        Gripped,
        GrippedMoving,
        Venting,
    }

    private void Awake()
    {
        defaultControllerHeight = CharacterController.height;
        defaultControllerRadius = CharacterController.radius;
        ChangeState(States.Move);
    }

    private void Start()
    {
        var environmentInfo = GetComponent<EnvironmentInfo>();
        environmentInfo.OnFallAction += () =>
        {
            Stun(1f);
            Mover.TeleportTo(SafeGroundChecker.SafePosition);
        };

        if (PlayerHud)
        {
            Health.OnChange += (current, max) =>
            {
                PlayerHud.SetHealth(current, max);
            };
            PlayerHud.SetHealth(Health.CurrentHealth, Health.CurrentHealth);
        }
    }

    private void Update()
    {
        var rotatedDirection = cameraFollow.rotation * direction;

        var gripReady = gripCooldown.UpdateTimer();
        switch (currentState)
        {
            case States.Move:
                Mover.Move(rotatedDirection);

                if (gripReady)
                {
                    if (Mover.IsGrounded == false && Gripper.Check())
                    {
                        ChangeState(States.Gripped);
                    }
                }
                break;

            case States.Fall:
                if (Mover.IsGrounded)
                {
                    ChangeState(States.Move);
                }
                else
                {
                    Mover.Move(Vector3.zero);
                }
                break;
            case States.Path:
                currentPathFollower.Move(input, interacting, rotatedDirection);
                if (currentPathFollower.Attatched == false)
                {
                    ChangeState(States.Move);
                }

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

                if (Gripper.IsInWall)
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
                if (Gripper.MoveAlongWall() == false)
                {
                    ChangeState(States.Fall);
                };
                break;
            case States.Venting:
                Venter.Move(cameraFollow.rotation * direction);

                if (Venter.IsInEntry == false && Venter.IsInside == false)
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
                if (Mover.IsGrounded)
                    Mover.Jump();
                break;
            case States.Path:
                ChangeState(States.Move);
                Mover.Jump();
                break;
            case States.Gripped:
                Mover.Launch(Vector3.Lerp(Gripper.GripNormal, Vector3.up, wallJumpUpFactor) * wallJumpSpeed);
                ChangeState(States.Move);
                break;
            case States.GrippedMoving:
                var moveDirection = Gripper.MoveVelocity;
                moveDirection.y = 0;
                moveDirection.Normalize();

                var jumpDirection = Vector3.Lerp(Gripper.GripNormal, moveDirection, 0.75f);

                Mover.Launch(Vector3.Lerp(jumpDirection, Vector3.up, movingWallJumpUpFactor) * wallJumpSpeed);
                ChangeState(States.Move);

                break;

            case States.Venting:
                break;

        }
    }

    public void OnInteract()
    {
        if (interacting == false)
        {
            interacting = true;
        }
        else
        {
            interacting = false;
            return;
        }

        if (Mover.IsGrounded == false && Roper.Check())
        {
            UsePathFollower(Roper);
        }
        else if (Mover.IsGrounded == false && Piper.Check())
        {
            UsePathFollower(Piper);
        }
        else if (Mover.IsGrounded == false && FallRoper.Check())
        {
            UsePathFollower(FallRoper);
        }
        else if (WallSticker.Check())
        {
            UsePathFollower(WallSticker);
        }
        else if (Venter.Check())
        {
            ChangeState(States.Venting);
        }
    }

    public void OnHit()
    {
        if (Meleer.hitting == false)
            Meleer.Hit();
    }
    void UsePathFollower(IPathFollower follower)
    {
        currentPathFollower = follower;
        follower.Attach();
        Align(follower.GetClosestPoint(), States.Path);
    }

    void Align(Vector3 position, States nextState)
    {
        ChangeState(States.Aligning);
        Aligner.Align(position, () =>
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
                Mover.enabled = true;
                Aligner.enabled = false;
                Gripper.enabled = false;
                Venter.enabled = false;

                gripCooldown.ResetTimer();

                CharacterController.height = defaultControllerHeight;
                CharacterController.radius = defaultControllerRadius;

                CameraSelector.UseFollowCamera();
                break;
            case States.Fall:
                Mover.enabled = true;
                Aligner.enabled = false;
                Gripper.enabled = false;
                Venter.enabled = false;

                CharacterController.height = defaultControllerHeight;
                CharacterController.radius = defaultControllerRadius;

                CameraSelector.UseFollowCamera();
                break;

            case States.Path:
                Mover.enabled = false;
                Aligner.enabled = false;
                Gripper.enabled = false;
                Venter.enabled = false;

                CharacterController.height = defaultControllerHeight;
                CharacterController.radius = defaultControllerRadius;

                CameraSelector.UseFollowCamera();
                break;

            case States.Stunned:
                Mover.enabled = true;
                Aligner.enabled = false;
                Gripper.enabled = false;
                Venter.enabled = false;

                CharacterController.height = defaultControllerHeight;
                CharacterController.radius = defaultControllerRadius;

                CameraSelector.UseFollowCamera();
                break;
            case States.Aligning:
                Mover.enabled = false;
                Aligner.enabled = true;
                Gripper.enabled = false;
                Venter.enabled = false;

                CharacterController.height = defaultControllerHeight;
                CharacterController.radius = defaultControllerRadius;

                CameraSelector.UseFollowCamera();
                break;

            case States.Gripped:
                Mover.enabled = false;
                Aligner.enabled = false;
                Gripper.enabled = true;
                Venter.enabled = false;

                gripStayTimer.ResetTimer();
                gripStartMoveTimer.ResetTimer();
                gripCooldown.ResetTimer();

                CharacterController.height = defaultControllerHeight;
                CharacterController.radius = defaultControllerRadius;

                CameraSelector.UseFollowCamera();
                break;
            case States.GrippedMoving:
                Mover.enabled = false;
                Aligner.enabled = false;
                Gripper.enabled = true;
                Venter.enabled = false;

                gripCooldown.ResetTimer();
                Gripper.SetMoveDirection(cameraFollow.rotation * direction);

                CharacterController.height = defaultControllerHeight;
                CharacterController.radius = defaultControllerRadius;

                CameraSelector.UseFollowCamera();
                break;

            case States.Venting:
                Mover.enabled = false;
                Aligner.enabled = false;
                Gripper.enabled = false;
                Venter.enabled = true;

                CharacterController.height = ventControllerHeight;
                CharacterController.radius = ventControllerRadius;

                CameraSelector.UseVentCamera();
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
