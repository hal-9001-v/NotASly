using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Mover))]
public class Player : MonoBehaviour
{
    [SerializeField] Transform cameraFollow;

    Mover mover => GetComponent<Mover>();

    [Header("Movement")]
    [SerializeField] float cameraSpeed = 5f;

    Vector2 cameraDirection;

    private void LateUpdate()
    {
        cameraFollow.Rotate(Vector3.up, cameraDirection.x * cameraSpeed * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        var input = value.Get<Vector2>();
        var direction = new Vector3(input.x, 0, input.y);

        mover.Move(cameraFollow.rotation * direction);
    }

    public void OnCamera(InputValue value)
    {
        cameraDirection = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if(mover.IsGrounded)
        {
            mover.Jump();
        }
    }

}
