using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripPoint : MonoBehaviour
{
    [SerializeField] Transform normalPivot;

    [SerializeField] TriggerInteractable triggerInteractable;
    public Vector3 Normal => (normalPivot.position - transform.position).normalized;

    private void Start()
    {
        triggerInteractable.OnEnterAction += Grip;
    }

    private void Grip(Interactor interactor)
    {
        var gripper = interactor.GetComponent<Gripper>();
        if (gripper != null)
        {
              gripper.Grip(this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, normalPivot.position);
        Gizmos.DrawSphere(normalPivot.position, 0.5f);
    }
}
