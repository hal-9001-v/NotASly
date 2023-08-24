using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Rope : MonoBehaviour
{
    SplineContainer splineContainer => GetComponent<SplineContainer>();

    public Vector3 GetPosition(float t)
    {
        return splineContainer.EvaluatePosition(t);
    }

    public Vector3 GetPosition(float t, out float newT, Vector3 direction, float displacement)
    {
        var sign = GetSign(t, direction);
        Debug.Log(sign);
        var delta = displacement / splineContainer.Spline.GetLength();
        newT = t + delta * sign;
        return splineContainer.EvaluatePosition(newT);
    }

    public float GetSign(float t, Vector3 direction)
    {
        var tangent = splineContainer.EvaluateTangent(t);
        return Mathf.Sign(Vector3.Dot(tangent, direction));
    }

    public float GetClosestT(Vector3 position)
    {
        float closest = -1;
        float closestDistance = float.MaxValue;
        for (float i = 0; i < 1; i += 0.05f)
        {
            var pos = splineContainer.EvaluatePosition(i);
            var newDistance = Vector3.Distance(pos, position);
            if (newDistance < closestDistance)
            {
                closest = i;
                closestDistance = newDistance;
            }
        }

        return closest;
    }

    public Vector3 GetClosestPoint(Vector3 position)
    {
        return splineContainer.EvaluatePosition(GetClosestT(position));
    }
}
