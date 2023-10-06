using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GroundCheck))]
public class Launchable : MonoBehaviour
{
    [SerializeField] float gravity = 20f;
    Rigidbody Rigidbody => GetComponent<Rigidbody>();
    GroundCheck GroundCheck => GetComponent<GroundCheck>();

    public Action OnLaunch;
    public Action OnStopLaunch;

    public Vector3 Velocity { get; private set; }

    bool isLaunched;

    public void Launch(Vector3 velocity)
    {
        OnLaunch?.Invoke();
        Velocity = velocity;
        isLaunched = true;
    }


    private void FixedUpdate()
    {
        if (isLaunched)
        {
            Velocity += Vector3.down * gravity * Time.fixedDeltaTime;
            Rigidbody.velocity = Velocity;

            GroundCheck.SetYSpeed(Velocity.y);
            if (GroundCheck.IsGrounded)
            {
                isLaunched = false;
                OnStopLaunch?.Invoke();
            }
        }
    }
}
