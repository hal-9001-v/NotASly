using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Path))]
public class Pipe : MonoBehaviour, IPathInteractable
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

    public float UpdatePosition(float t, float displacement, bool up)
    {
        if (up)
            displacement = Mathf.Abs(displacement);
        else
            displacement = -Mathf.Abs(displacement);

        return Path.GetTWithDisplacement(t, displacement);
    }

}
