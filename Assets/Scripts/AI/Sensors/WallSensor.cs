using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSensor : BaseSensor
{
    [SerializeField] LayerMask wallLayer;
    [SerializeField] float distance = 1f;

    [SerializeField] Vector3 direction;

    public bool IsTouchingWall { get; private set; }

    protected override void Check()
    {
        if(Physics.Raycast(transform.position, transform.TransformVector(direction), distance, wallLayer))
        {
            OnSense?.Invoke(null);
            IsTouchingWall = true;
        }
        else
        {
            IsTouchingWall = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.TransformVector(direction) * distance);
        Gizmos.DrawCube(transform.position + transform.TransformVector(direction) * distance, Vector3.one * 0.1f);

    }

}
