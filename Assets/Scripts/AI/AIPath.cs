using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AIPath : MonoBehaviour
{
    LineRenderer LineRenderer => GetComponent<LineRenderer>();

    [SerializeField][Range(0.1f, 5f)] float controlPointArea = 1f;
    Vector3[] controlPoints;

    void Awake()
    {
        controlPoints = new Vector3[LineRenderer.positionCount];


        LineRenderer.GetPositions(controlPoints);

        if(LineRenderer.useWorldSpace == false)
        {
            for (int i = 0; i < controlPoints.Length; i++)
            {
                controlPoints[i] = transform.TransformPoint(controlPoints[i]);
            }
        }
    }

    public bool IsInsideControlPoint(int i, Vector3 position)
    {
        if (i < 0 || i >= controlPoints.Length) return false;

        if (Vector3.Distance(controlPoints[i], position) < controlPointArea) return true;

        return false;
    }

    public Vector3 GetClosestControlPoint(Vector3 position)
    {
        return controlPoints.OrderBy((a) => Vector3.Distance(position, a)).First();
    }

    public int GetClosestControlPointIndex(Vector3 position)
    {
        return controlPoints.Select((a, i) => new {a, i}).OrderBy((a) => Vector3.Distance(position, a.a)).First().i;
    }

    public Vector3 GetControlPoint(int i)
    {
        if (i < 0 || i >= controlPoints.Length) return Vector3.zero;

        return controlPoints[i];
    }

    public int NextPoint(int i)
    {
        if (i < 0 || i >= controlPoints.Length) return -1;

        return (i + 1) % controlPoints.Length;
    }

    private void OnDrawGizmosSelected()
    {
        var positions = new Vector3[LineRenderer.positionCount];
        LineRenderer.GetPositions(positions);
        if(LineRenderer.useWorldSpace == false)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = transform.TransformPoint(positions[i]);
            }
        }

        Gizmos.color = Color.red;
        foreach (var point in positions)
        {
            Gizmos.DrawSphere(point, controlPointArea * 0.2f);
            Gizmos.DrawWireSphere(point, controlPointArea);
        }
    }


}
