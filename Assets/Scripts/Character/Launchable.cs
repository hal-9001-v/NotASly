using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Launchable : MonoBehaviour
{
    Rigidbody Rigidbody => GetComponent<Rigidbody>();

    public Func<bool> CanLaunch;

    public Action OnLaunch;
    public Action OnStopLaunch;

    public void Launch(Vector3 force)
    {
        OnLaunch?.Invoke();
        Rigidbody.AddForce(force, ForceMode.Impulse);
    }


    private void FixedUpdate()
    {
        
    }
}
