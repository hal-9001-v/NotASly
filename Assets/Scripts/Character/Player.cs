using System.Collections;
using System.Linq;
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
    FirstPerson FirstPerson => GetComponent<FirstPerson>();
    Venter Venter => GetComponent<Venter>();
    PickPocketer PickPocketer => GetComponent<PickPocketer>();
    Interactor Interactor => GetComponent<Interactor>();
    Rigidbody Rigidbody => GetComponent<Rigidbody>();
    CharacterController CharacterController => GetComponent<CharacterController>();
    SafeGroundChecker SafeGroundChecker => GetComponent<SafeGroundChecker>();
    CameraSelector CameraSelector => FindObjectOfType<CameraSelector>();
    PlayerHud PlayerHud => FindObjectOfType<PlayerHud>();

    PlayerMenace[] Menaces => FindObjectsOfType<PlayerMenace>();
    GroundCheck GroundCheck => GetComponent<GroundCheck>();
    GameOver GameOver => FindObjectOfType<GameOver>();

    Pause Pause => FindObjectOfType<Pause>();

    [SerializeField] float cameraSpeed = 5f;
    [SerializeField] float mouseCameraSpeed = 5f;
    [SerializeField] float maxMenaceRadius = 5;
    [SerializeField] float minMenaceRadius = 2.5f;

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
        FirstPerson,
        Launched,
        Aligning,
        Stunned,
        Gripped,
        Venting,
    }



    private void Awake()
    {
        ChangeState(States.Move);
        Cursor.lockState = CursorLockMode.Locked;
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

        SafeGroundChecker.OnSetSafe += (pos) =>
        {
            var controllerEnabled = CharacterController.enabled;
            var kinematic = Rigidbody.isKinematic;

            CharacterController.enabled = false;
            Rigidbody.isKinematic = true;

            transform.position = pos;

            CharacterController.enabled = controllerEnabled;
            Rigidbody.isKinematic = kinematic;
            ChangeState(States.Fall);
        };

        Health.OnDead += (pos) =>
        {
            GameOver.Show();
        };
    }

    void OnLaunch()
    {
        Rigidbody.isKinematic = false;
        CharacterController.enabled = false;
        ChangeState(States.Launched);
    }

    void OnStopLaunch()
    {
        Rigidbody.isKinematic = true;
        CharacterController.enabled = true;
        ChangeState(States.Move);
    }

    Transform GetClosestMenace()
    {
        //Get the closest menace
        var closestMenace = Menaces.OrderBy(m => Vector3.Distance(m.transform.position, transform.position)).FirstOrDefault();

        if (closestMenace && Vector3.Distance(closestMenace.transform.position, transform.position) < maxMenaceRadius)
        {
            return closestMenace.transform;
        }

        return null;

    }

    private void Update()
    {
        var rotatedDirection = cameraFollow.rotation * direction;

        switch (currentState)
        {
            case States.Move:
                if (rotatedDirection.magnitude > 0.1f)
                {
                    var closestMenace = GetClosestMenace();
                    if (closestMenace)
                    {
                        var toMenace = closestMenace.position - transform.position;
                        toMenace.y = 0;
                        toMenace.Normalize();

                        var rotation = Quaternion.LookRotation(toMenace, Vector3.up);
                        var menaceMovement = rotation * new Vector3(input.x, 0, input.y);
                        Debug.DrawLine(transform.position, transform.position + menaceMovement * 2, Color.red);

                        var distance = Vector3.Distance(closestMenace.position, transform.position);
                        var menaceFactor = Mathf.Clamp01((maxMenaceRadius - distance) / (maxMenaceRadius - minMenaceRadius));

                        Mover.Move(Vector3.Lerp(rotatedDirection,menaceMovement, menaceFactor), false);
                        Mover.Steer(toMenace);
                    }
                    else
                    {
                        Mover.Steer(rotatedDirection);
                        Mover.MoveForward();
                    }
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

            case States.FirstPerson:
                FirstPerson.Move(cameraDirection);
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

                if (Venter.Check() == false)
                {
                    Venter.Release();
                    ChangeState(States.Move);
                }
                break;
            case States.Launched:
                break;
        }

    }

    private void LateUpdate()
    {
        cameraFollow.Rotate(Vector3.up, cameraDirection.x * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        direction = new Vector3(input.x, 0, input.y);
    }

    public void OnCamera(InputValue value)
    {
        cameraDirection = value.Get<Vector2>() * cameraSpeed;

    }

    public void OnMouseCamera(InputValue value)
    {
        cameraDirection = value.Get<Vector2>() * mouseCameraSpeed;
    }

    public void OnBinocucom()
    {
        if (currentState == States.FirstPerson)
        {
            FirstPerson.Dettach();
            ChangeState(States.Move);
        }
        else if (currentState == States.Move)
        {
            FirstPerson.Attatch();
            ChangeState(States.FirstPerson);
        }
    }

    public void OnFovDelta(InputValue value)
    {
        if (currentState == States.FirstPerson)
        {
            FirstPerson.ChangeFov(value.Get<float>());
        }
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

        switch (currentState)
        {
            case States.Move:

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

                    if (Interactor.ActiveInteraction())
                    {

                    }
                    else if (PickPocketer.Check())
                    {
                        PickPocketer.Poke();
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

                break;
            case States.Path:

                break;
        }

    }

    public void OnPause()
    {
        Pause.Switch();
    }

    public void OnHit()
    {
        switch (currentState)
        {
            case States.Move:
                if (Meleer.hitting == false)
                {
                    Meleer.Hit();
                    PlayerAnimator.Hit();
                }
                break;
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
                Launchable.enabled = false;
                FirstPerson.enabled = false;

                CameraSelector.UseFollowCamera();
                break;

            case States.Fall:
                Mover.enabled = true;
                Aligner.enabled = false;
                Gripper.enabled = false;
                Venter.enabled = false;
                Launchable.enabled = false;
                FirstPerson.enabled = false;

                CameraSelector.UseFollowCamera();
                break;

            case States.Path:
                Mover.enabled = false;
                Aligner.enabled = false;
                Gripper.enabled = false;
                Venter.enabled = false;
                Launchable.enabled = false;
                FirstPerson.enabled = false;

                CameraSelector.UseFollowCamera();
                break;

            case States.Point:
                Mover.enabled = false;
                Aligner.enabled = false;
                Gripper.enabled = false;
                Venter.enabled = false;
                Launchable.enabled = false;
                FirstPerson.enabled = false;

                CameraSelector.UseFollowCamera();
                break;
            case States.Stunned:
                Mover.enabled = true;
                Aligner.enabled = false;
                Gripper.enabled = false;
                Venter.enabled = false;
                Launchable.enabled = false;
                FirstPerson.enabled = false;

                CameraSelector.UseFollowCamera();
                break;

            case States.FirstPerson:
                Mover.enabled = false;
                Aligner.enabled = false;
                Gripper.enabled = false;
                Venter.enabled = false;
                Launchable.enabled = false;
                FirstPerson.enabled = true;
                break;

            case States.Aligning:
                Mover.enabled = false;
                Aligner.enabled = true;
                Gripper.enabled = false;
                Venter.enabled = false;
                Launchable.enabled = false;
                FirstPerson.enabled = false;

                CameraSelector.UseFollowCamera();
                break;

            case States.Gripped:
                Mover.enabled = false;
                Aligner.enabled = false;
                Gripper.enabled = true;
                Venter.enabled = false;
                Launchable.enabled = false;
                FirstPerson.enabled = false;

                CameraSelector.UseFollowCamera();
                break;

            case States.Venting:
                Mover.enabled = false;
                Aligner.enabled = false;
                Gripper.enabled = false;
                Venter.enabled = true;
                FirstPerson.enabled = false;

                Venter.Attach();

                CameraSelector.UseVentCamera();
                break;

            case States.Launched:
                Mover.enabled = false;
                Aligner.enabled = false;
                Gripper.enabled = false;
                Venter.enabled = false;
                Launchable.enabled = true;
                FirstPerson.enabled = false;

                CameraSelector.UseFollowCamera();
                break;

            default:
                Debug.LogError("Agrega el nuevo estado melón!");
                break;
        }
        currentState = newState;
    }

    IEnumerator StunCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        ChangeState(States.Move);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxMenaceRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minMenaceRadius);
    }
}
