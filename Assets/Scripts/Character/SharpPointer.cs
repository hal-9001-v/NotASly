using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpPointer : MonoBehaviour
{
    [SerializeField][Range(0, 5)] float checkRadius = 1f;
    SharpPoint[] SharpPoints => FindObjectsOfType<SharpPoint>();

    public bool Check()
    {
        var closest = GetClosest();
        if (closest)
        {
            if (HorizontalDistance(transform.position, closest.Point) < checkRadius)
            {
                return true;
            }
        }

        return false;
    }

    public SharpPoint GetClosest()
    {
        SharpPoint closest = null;
        float closestDistance = float.MaxValue;
        foreach (var point in SharpPoints)
        {

            if (point.Point.y < transform.position.y && HorizontalDistance(transform.position, point.Point) < closestDistance)
            {
                closest = point;
                closestDistance = Vector3.Distance(transform.position, point.Point);
            }
        }

        return closest;
    }

    float HorizontalDistance(Vector3 a, Vector3 b)
    {
        return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
    }

}
