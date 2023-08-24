using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.ProBuilder;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(CharacterController))]
public class Gripper : MonoBehaviour
{

    [SerializeField] LayerMask gripLayer;
    [SerializeField] float gripDistance = 1f;
    [SerializeField][Range(0, 20)] float wallSpeed = 5f;
    [SerializeField] AnimationCurve gripGravityProgression;
    [SerializeField][Range(0, 10)] float minVelocity = 1f;
    [SerializeField] Timer timeInWall;
    [SerializeField][Range(0, 1)] float distanceToWall = 0.15f;
    [SerializeField][Range(0, 1)] float pressedTime = 0.5f;
    [SerializeField][Range(0, 1)] float deadzone = 0.2f;
    [SerializeField] float gravity = 20;

    Vector3 gripPosition;

    GrippableWall currentWall;
    Vector3 moveVelocity;
    public Vector3 GripNormal { get => currentWall.Normal; }

    CharacterController characterController => GetComponent<CharacterController>();

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
                transform.position = gripPosition + GripNormal * distanceToWall;
                break;
            case States.Moving:
                timeInWall.UpdateTimer();

                moveVelocity.y -= gripGravityProgression.Evaluate(timeInWall.NormalizedTime) * gravity * Time.fixedDeltaTime;
                characterController.Move(moveVelocity * Time.fixedDeltaTime);
                break;
        }
    }

    public void Grip(GrippableWall grippableWall)
    {
        currentWall = grippableWall;
    }

    public void Release()
    {
        currentWall = null;
    }

    public bool Check()
    {
        if (currentWall == null)
        {
            return false;
        }

        if (Physics.Raycast(transform.position, -currentWall.Normal, out RaycastHit hit, gripDistance, gripLayer))
        {
            gripPosition = hit.point;
            currentState = States.Idle;
            return true;
        }

        return false;
    }



    public void SetMoveDirection(Vector3 direction)
    {
        currentState = States.Moving;
        timeInWall.ResetTimer();

        if (Vector3.Dot(direction, currentWall.Right) > 0)
        {
            moveVelocity = currentWall.Right;
        }
        else
        {
            moveVelocity = -currentWall.Right;
        }

        moveVelocity *= wallSpeed;
    }

    public bool MoveAlongWall()
    {
        Debug.DrawRay(transform.position + moveVelocity.normalized, -GripNormal, Color.red);
        if (Physics.Raycast(transform.position, -GripNormal, out RaycastHit hit, 5, gripLayer))
        {
            return true;
        }

        return false;
    }
}
