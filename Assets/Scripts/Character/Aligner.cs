using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aligner : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity = 20;
    [SerializeField] float alignSpeed = 10f;

    Vector3 startPosition;
    Vector3 endPosition;

    float elapsedTime;
    float time = -1;

    float yPos;
    float yVelocity;

    Action callbackAction;

    bool aligning;

    private void Awake()
    {
        aligning = false;
    }

    public void Align(Vector3 position, Action callback)
    {
        callbackAction = callback;

        startPosition = transform.position;
        endPosition = position;

        var jumpCorrection = endPosition.y - startPosition.y;
        if (jumpCorrection < 0)
        {
            jumpCorrection = 0;
        }

        yVelocity = Mathf.Sqrt(2 * gravity * (jumpHeight + jumpCorrection));
        yPos = startPosition.y;

        elapsedTime = 0;
        time = Vector3.Distance(startPosition, endPosition) / alignSpeed;
        aligning = true;
    }

    private void FixedUpdate()
    {
        if (aligning)
        {
            if (elapsedTime < time)
            {
                elapsedTime += Time.fixedDeltaTime;
            }

            var pos = Vector3.Lerp(startPosition, endPosition, elapsedTime / time);

            yVelocity -= gravity * Time.fixedDeltaTime;
            yPos += yVelocity * Time.fixedDeltaTime;
            pos.y = yPos;

            transform.position = pos;
            
            if (yPos <= endPosition.y)
            {
                pos.y = endPosition.y;
                time = -1;
                callbackAction?.Invoke();
                aligning = false;
            }
        }
    }
}
