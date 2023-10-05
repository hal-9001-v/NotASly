using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Meleer))]
[RequireComponent(typeof(Piper))]
[RequireComponent(typeof(SharpPointer))]
[RequireComponent(typeof(FallRoper))]
[RequireComponent(typeof(WallSticker))]
[RequireComponent(typeof(Gripper))]
[RequireComponent(typeof(EnvironmentInfo))]
[RequireComponent(typeof(SafeGroundChecker))]
[RequireComponent(typeof(Roper))]
[RequireComponent(typeof(Aligner))]
[RequireComponent(typeof(Venter))]
public class Player : MonoBehaviour
{
    [SerializeField] Transform cameraFollow;

    PlayerAnimator PlayerAnimator => FindObjectOfType<PlayerAnimator>();
    Health Health => GetComponent<Health>();
    Mover Mover => GetComponent<Mover>();
    Meleer Meleer => GetComponent<Meleer>();
    Roper Roper => GetComponent<Roper>();
    Piper Piper => GetComponent<Piper>();
    SharpPointer SharpPointer => GetComponent<SharpPointer>();
    FallRoper FallRoper => GetComponent<FallRoper>();
    WallSticker WallSticker => GetComponent<WallSticker>();
    Gripper Gripper => GetComponent<Gripper>();
    Aligner Aligner => GetComponent<Aligner>();
    Launchable Launchable => GetComponent<Launchable>();
    Venter Venter => GetComponent<Venter>();

    CharacterController CharacterController => GetComponent<CharacterController>();
    SafeGroundChecker SafeGroundChecker => GetComponent<SafeGroundChecker>();
    CameraSelector CameraSelector => FindObjectOfType<CameraSelector>();
    PlayerHud PlayerHud => FindObjectOfType<PlayerHud>();

    GroundCheck GroundCheck => GetComponent<GroundCheck>();

    [Header("Movement")]
    [SerializeField] float cameraSpeed = 5f;
    [SerializeField] float wallJumpSpeed = 20;

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
    bool doubleJumpReady;

    IPathFollower currentPathFollower;
    enum States
    {
        None,
        Move,
        Fall,
        Path,
        Point,
        Launched,
        Aligning,
        Stunned,
        Gripped,
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

        Launchable.OnLaunch += OnLaunch;
        Launchable.OnStopLaunch += OnStopLaunch;
    }

    void OnLaunch()
    {
        ChangeState(States.Launched);
    }

    void OnStopLaunch()
    {
        ChangeState(States.Move);
    }

    private void Update()
    {
        var rotatedDirection = cameraFollow.rotation * direction;

        switch (currentState)
        {
            case States.Move:
                if (rotatedDirection.magnitude > 0.1f)
                {
                    Mover.Move(rotatedDirection);
                    if (GroundCheck.IsGrounded)
                        PlayerAnimator.Walk();
                    else
                        PlayerAnimator.Jump();
                }
                else
                {
                    Mover.Move(Vector3.zero);
                    if (GroundCheck.IsGrounded)
                        PlayerAnimator.Idle();
                    else
                        PlayerAnimator.Jump();
                }

                if (Gripper.Check())
                {
                    Gripper.Attatch();
                    ChangeState(States.Gripped);
                }

                break;

            case States.Fall:
                if (GroundCheck.IsGrounded)
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
 
            case States.Point:
                Mover.Steer(rotatedDirection);
                break;
        
            case States.Aligning:

                break;
            
            case States.Stunned:
                break;
            
            case States.Gripped:
                if (Gripper.IsGripped)
                {
                    Gripper.Move(rotatedDirection);
                }
                else
                {
                    
                    Gripper.Release();
                    ChangeState(States.Move);
                }

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
                Jump(false);
                break;
            case States.Path:
                Jump(true);
                break;
            case States.Point:
                Jump(true);
                break;
            case States.Gripped:
                Gripper.Jump();
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

        if (GroundCheck.IsGrounded)
        {
            if (WallSticker.Check())
            {
                UsePathFollower(WallSticker);
            }
            else if (Venter.Check())
            {
                ChangeState(States.Venting);
            }
        }
        else
        {
            if (Roper.Check())
            {
                UsePathFollower(Roper);
            }
            else if (Piper.Check())
            {
                UsePathFollower(Piper);
            }
            else if (FallRoper.Check())
            {
                UsePathFollower(FallRoper);
            }
            else if (SharpPointer.Check())
            {
                Align(SharpPointer.GetClosest().Point, States.Point);
            }
        }

    }

    public void OnHit()
    {
        if (Meleer.hitting == false)
        {
            Meleer.Hit();
            PlayerAnimator.Hit();
        }
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

    void Jump(bool force)
    {
        ChangeState(States.Move);

        if (GroundCheck.IsGrounded || force)
        {
            doubleJumpReady = true;
            Mover.Jump();
            PlayerAnimator.Jump();
        }
        else if (doubleJumpReady)
        {
            doubleJumpReady = false;
            Mover.DoubleJump();
            PlayerAnimator.Jump();
        }
    }

    void ChangeState(States newState)
    {
        if (currentState == newState)
            return;

        switch (newState)
        {
            case States.Move:
                Mover.enabled = true;
                Aligner.enabled = false;
                Gripper.enabled = false;
                Venter.enabled = false;

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

            case States.Point:
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
                Launchable.enabled = false;

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
