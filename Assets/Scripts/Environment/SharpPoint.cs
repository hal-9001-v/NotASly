using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpPoint : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Vector3 offset;

    [Header("Debug")]
    [SerializeField][Range(0, 1)] float gizmosSize = 0.1f;
    public Vector3 Point => transform.position + offset;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Point, gizmosSize);
    }
}
