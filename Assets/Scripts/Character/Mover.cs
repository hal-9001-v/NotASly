using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour
{
    CharacterController controller => GetComponent<CharacterController>();

    [Header("Movement")]
    [SerializeField] float speed = 6f;
    [SerializeField] float jumpHeight = 5;
    [SerializeField] float gravity = -20;
    [SerializeField] LayerMask groundMask;

    float ySpeed;

    Vector3 direction;
    Vector3 launchVelocity;

    public bool IsGrounded
    {
        get
        {
            if (Physics.Raycast(transform.position, Vector3.down, out var hit, controller.height * 0.55f, groundMask))
            {
                return true;
            }

            return false;
        }
    }

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
        if (IsGrounded && ySpeed < 0)
        {
            ySpeed = 0f;
            launchVelocity = Vector3.zero;
        }

        Vector3 velocity;
        if (launchVelocity.magnitude > 0.1f)
        {
            velocity = ySpeed * Vector3.up + launchVelocity;

        }
        else
        {
            velocity = direction * speed + ySpeed * Vector3.up;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    public void Move(Vector3 direction)
    {
        this.direction = direction;
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

    public void Jump(float height)
    {
        ySpeed = Mathf.Sqrt(height * -2f * gravity);
    }

    public void Launch(Vector3 velocity)
    {
        ySpeed = velocity.y;

        launchVelocity = velocity;
        launchVelocity.y = 0;
    }
}
