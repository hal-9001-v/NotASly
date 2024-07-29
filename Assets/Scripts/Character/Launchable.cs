using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GroundCheck))]
public class Launchable : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float gravity = 20f;
    [SerializeField] bool turnKinenticOnStop = true;
    Rigidbody Rigidbody => GetComponent<Rigidbody>();
    GroundCheck GroundCheck => GetComponent<GroundCheck>();

    public Action OnLaunch;
    public UnityEvent OnLaunchEvent;
    public Action OnStopLaunch;
    public UnityEvent OnStopLaunchEvent;

    public Vector3 Velocity { get; private set; }

    bool isLaunched;

    private void Awake()
    {
        if (turnKinenticOnStop)
        {
            Rigidbody.isKinematic = true;
        }
    }

    public void Launch(Vector3 velocity)
    {
        OnLaunch?.Invoke();
        OnLaunchEvent?.Invoke();
        Velocity = velocity;
        isLaunched = true;

        if (turnKinenticOnStop)
        {
            Rigidbody.isKinematic = false;
        }

    }


    private void FixedUpdate()
    {
        if (isLaunched)
        {
            Velocity += Vector3.down * gravity * Time.fixedDeltaTime;
            Rigidbody.linearVelocity = Velocity;

            GroundCheck.SetYSpeed(Velocity.y);
            if (GroundCheck.IsGrounded)
            {
                Rigidbody.isKinematic = true;
                isLaunched = false;
                OnStopLaunch?.Invoke();
                OnStopLaunchEvent?.Invoke();
            }
        }
    }
}
