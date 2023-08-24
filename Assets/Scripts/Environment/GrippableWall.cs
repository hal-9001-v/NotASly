using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrippableWall : MonoBehaviour
{
    [SerializeField] Transform normalPivot;
    [SerializeField] TriggerInteractable triggerInteractable;

    public Vector3 Normal => (normalPivot.position - transform.position).normalized;
    public Vector3 Right => Vector3.Cross(Normal, Vector3.up).normalized;
    private void Start()
    {
        triggerInteractable.OnEnterAction += Grip;
        triggerInteractable.OnExitAction += Release;
    }

    void Grip(Interactor interactor)
    {
        var gripper = interactor.GetComponent<Gripper>();

        if (gripper)
        {
            gripper.Grip(this);
        }
    }

    void Release(Interactor interactor)
    {
        var gripper = interactor.GetComponent<Gripper>();

        if (gripper)
        {
            gripper.Release();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (normalPivot == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, normalPivot.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(normalPivot.position, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(normalPivot.position, normalPivot.position +  Right);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(normalPivot.position + Right, 0.25f);
    }
}
