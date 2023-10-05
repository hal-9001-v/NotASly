using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform rotatingPivot;
    CharacterController controller => GetComponent<CharacterController>();
    GroundCheck GroundCheck => GetComponentInChildren<GroundCheck>();
    [Header("Movement")]
    [SerializeField] float speed = 6f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float jumpHeight = 5;
    [SerializeField] float doubleJumpHeight = 5;
    [SerializeField] float gravity = -20;

    [SerializeField][Range(0, 720)] float rotatingSpeed;

    [SerializeField] LayerMask groundMask;

    float currentSpeed;
    float ySpeed;

    Vector3 direction;
    Vector3 launchVelocity;

   
    private void OnDisable()
    {
        direction = Vector3.zero;
        launchVelocity = Vector3.zero;
        controller.Move(Vector3.zero);
        ySpeed = 0;
    }

    private void OnEnable()
    {
        launchVelocity = controller.velocity;
        ySpeed = launchVelocity.y;
        launchVelocity.y = 0;
    }

    private void FixedUpdate()
    {
        ySpeed += gravity * Time.deltaTime;
        GroundCheck.SetYSpeed(ySpeed);
        if (GroundCheck.IsGrounded)
        {
            ySpeed = 0f;
            launchVelocity = Vector3.zero;
        }

        Steer(direction);

        var velocity = ySpeed * Vector3.up;
        if (launchVelocity.magnitude > 0.1f)
        {
            velocity += launchVelocity;

        }
        else
        {
            if (direction.magnitude > 0.1f)
            {
                currentSpeed += acceleration * Time.fixedDeltaTime;
                if (currentSpeed > speed)
                {
                    currentSpeed = speed;
                }

                velocity += rotatingPivot.forward * currentSpeed;
            }
            else
            {
                currentSpeed = 0;
            }
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void Move(Vector3 direction)
    {
        this.direction = direction;
    }

    public void Steer(Vector3 direction, bool insta = false)
    {

        if (direction.magnitude < 0.1f)
        {
            return;
        }

        if(insta)
        {
            rotatingPivot.rotation = Quaternion.LookRotation(direction, Vector3.up);
            return;
        }

        var targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        var angle = Quaternion.Angle(rotatingPivot.rotation, targetRotation);

        rotatingPivot.rotation = Quaternion.Lerp(rotatingPivot.rotation, targetRotation, Time.fixedDeltaTime * rotatingSpeed / angle);
    }

    public void TeleportTo(Vector3 position)
    {
        direction = Vector3.zero;

        controller.enabled = false;
        transform.position = position;
        controller.enabled = true;
    }

    public void Jump()
    {
        Jump(jumpHeight);
    }

    public void DoubleJump()
    {
        Jump(doubleJumpHeight);
    }

    public void Jump(float height)
    {
        ySpeed = Mathf.Sqrt(height * -2f * gravity);
    }

    public void Launch(Vector3 velocity)
    {
        enabled = true;

        ySpeed = velocity.y;

        launchVelocity = velocity;
        launchVelocity.y = 0;
    }
}
