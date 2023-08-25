using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Venter : MonoBehaviour
{

    [SerializeField][Range(1, 10)] float movingSpeed = 5;
    [SerializeField][Range(1, 100)] float gravity = 40;

    public bool IsInside;
    public bool IsInEntry;

    Vector3 direction;
    CharacterController characterController => GetComponent<CharacterController>();
    Mover mover => GetComponent<Mover>();

    float ySpeed;
    public bool Check()
    {
        if (IsInEntry)
        {
            return true;
        }

        return false;
    }

    public void Move(Vector3 direction)
    {
        this.direction = direction;
    }

    private void FixedUpdate()
    {
        ySpeed -= gravity * Time.fixedDeltaTime;

        if (mover.IsGrounded)
            ySpeed = 0;

        characterController.Move(direction * movingSpeed * Time.fixedDeltaTime + Vector3.up * ySpeed);
    }


}
