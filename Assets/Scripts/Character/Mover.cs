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

    public bool IsGrounded
    {
        get
        {
            if (Physics.Raycast(transform.position, Vector3.down, controller.height * 0.55f, groundMask))
            {
                return true;
            }

            return false;
        }
    }

    private void FixedUpdate()
    {
        ySpeed += gravity * Time.deltaTime;
        if (IsGrounded && ySpeed < 0)
        {
            ySpeed = 0f;
        }

        var velocity = direction * speed + ySpeed * Vector3.up;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Move(Vector3 direction)
    {
        this.direction = direction;
    }

    public void Jump()
    {
        Jump(jumpHeight);
    }

    public void Jump(float height)
    {
        ySpeed = Mathf.Sqrt(height * -2f * gravity);
    }



}
