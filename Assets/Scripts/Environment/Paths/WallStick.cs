using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStick : MonoBehaviour, IPathInteractable
{
    public Path Path => GetComponent<Path>();

    public Vector3 GetClosestPoint(Vector3 point)
    {
        return Path.GetClosestPoint(point);
    }

    public float GetClosestT(Vector3 point)
    {
        return Path.GetClosestT(point);
    }
}
