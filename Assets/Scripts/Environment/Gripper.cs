using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Gripper : MonoBehaviour
{
    [SerializeField] LayerMask gripLayer;
    [SerializeField] float gripDistance = 1f;
    [SerializeField][Range(0, 1)] float distanceToWall = 0.15f;

    [SerializeField][Range(0, 20)] float wallSpeed = 5f;

    [SerializeField][Range(0, 1)] float wallJumpUpFactor = 0.75f;
    [SerializeField][Range(0, 50)] float wallJumpSpeed = 0.75f;

    float idleFallSpeed;
    [SerializeField] float idleGravity = 20;
    [SerializeField] float gravity = 20;
    [SerializeField] AnimationCurve gripGravityProgression;

    [SerializeField] Timer timeInWall;
    [SerializeField] Timer startMovingTime;
    [SerializeField] Timer gripCooldown;

    Vector3 gripPosition;

    GrippableWall currentWall;
    GripPoint currentGripPoint;

    public bool IsGripped { get; private set; }

    Vector3 moveVelocity;

    public Vector3 MoveVelocity { get { return moveVelocity; } }

    public Vector3 GripNormal
    {
        get
        {
            if (currentWall)
                return currentWall.Normal;

            if (currentGripPoint)
                return currentGripPoint.Normal;

            return Vector3.zero;
        }
    }

    Mover Mover => GetComponent<Mover>();
    GroundCheck GroundCheck => GetComponentInChildren<GroundCheck>();

    States currentState;

    enum States
    {
        Idle,
        Moving,
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case States.Idle:
                Mover.Steer(GripNormal, true);
                GroundCheck.SetYSpeed(idleFallSpeed);
                if (GroundCheck.IsGrounded)
                {
                    Release();
                    return;
                }

                idleFallSpeed -= idleGravity * Time.fixedDeltaTime;
                if (UpdateGripPoint(Vector3.up * idleFallSpeed * Time.fixedDeltaTime))
                {
                    transform.position = gripPosition + GripNormal * distanceToWall;
                }
                else
                {
                    Release();
                }
                break;
            case States.Moving:
                Mover.Steer(moveVelocity.normalized, true);
                moveVelocity.y -= gripGravityProgression.Evaluate(timeInWall.NormalizedTime) * gravity * Time.fixedDeltaTime;
                GroundCheck.SetYSpeed(moveVelocity.y);

                if (GroundCheck.IsGrounded)
                {
                    Release();
                    return;
                }

                timeInWall.UpdateTimer();

                if (UpdateGripPoint(moveVelocity * Time.fixedDeltaTime))
                {
                    transform.position = gripPosition + GripNormal * distanceToWall;
                }
                else
                {
                    Release();
                }
                break;
        }
    }

    public void Move(Vector3 direction)
    {
        if (currentState == States.Idle)
        {
            if (currentWall)
            {
                if (direction.magnitude > 0.1f)
                {
                    if (startMovingTime.UpdateTimer())
                    {
                        if (Vector3.Dot(direction, currentWall.Right) > 0)
                        {
                            moveVelocity = currentWall.Right * wallSpeed;
                        }
                        else
                        {
                            moveVelocity = -currentWall.Right * wallSpeed;
                        }

                        currentState = States.Moving;
                    }
                }
                else
                {
                    startMovingTime.ResetTimer();
                }
            }
        }
    }

    public void Grip(GripPoint gripPoint)
    {
        currentGripPoint = gripPoint;
        currentWall = null;
    }

    public void Grip(GrippableWall grippableWall)
    {
        currentWall = grippableWall;
        currentGripPoint = null;
    }

    public void Attatch()
    {
        if (currentWall != null || currentGripPoint != null)
        {
            IsGripped = true;
            idleFallSpeed = 0;
            moveVelocity = Vector3.zero;

        }
    }

    public void Release()
    {
        currentWall = null;
        currentGripPoint = null;
        IsGripped = false;

        gripCooldown.ResetTimer();
    }

    public void Jump()
    {
        Mover.Launch(Vector3.Lerp(GripNormal, Vector3.up, wallJumpUpFactor) * wallJumpSpeed);
        Release();
    }

    public bool Check()
    {
        if (gripCooldown.IsFinished == false)
        {
            gripCooldown.UpdateTimer();
            return false;
        }

        if (currentWall == null && currentGripPoint == null)
        {
            return false;
        }

        if (GroundCheck.IsGrounded)
        {
            return false;
        }

        if (UpdateGripPoint(Vector3.zero))
        {
            currentState = States.Idle;
            return true;
        }

        return false;
    }

    bool UpdateGripPoint(Vector3 offset)
    {
        if (Physics.Raycast(transform.position + offset, -GripNormal, out RaycastHit hit, gripDistance, gripLayer))
        {
            gripPosition = hit.point;
            return true;
        }

        return false;
    }

}
