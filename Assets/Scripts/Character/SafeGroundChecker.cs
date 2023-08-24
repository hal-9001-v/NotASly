using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactor))]
public class SafeGroundChecker : MonoBehaviour
{
    SafeGround currentSafeGround;
    public Vector3 SafePosition => currentSafeGround.Position;

    public void SetSafeGround(SafeGround safeGround)
    {
        currentSafeGround = safeGround;
    }

    public void GetToSafeGround()
    {
        transform.position = currentSafeGround.Position;
    }
}
