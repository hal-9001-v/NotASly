using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactor))]
public class SafeGroundChecker : MonoBehaviour
{
    SafeGround currentSafeGround;
    public Vector3 SafePosition => currentSafeGround.Position;

    public Action<Vector3> OnSetSafe;

    public void SetSafeGround(SafeGround safeGround)
    {
        currentSafeGround = safeGround;
    }

    public void GetToSafeGround()
    {
        if(currentSafeGround == null)
        {
            Debug.LogError("No safe ground set");
            return;
        }

        transform.position = currentSafeGround.Position;
        OnSetSafe?.Invoke(currentSafeGround.Position);
    }
}
