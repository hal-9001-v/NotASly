using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerInteractable))]
public class SafeGround : MonoBehaviour
{
    [SerializeField] Transform safeGroundPosition;

    public Vector3 Position { get { return safeGroundPosition.position; } }

    void Start()
    {
        var triggerInteractable = GetComponent<TriggerInteractable>();
        triggerInteractable.OnEnterAction += SetSafeGround;
    }

    void SetSafeGround(Interactor interactor)
    {
        var safeGroundChecker = interactor.GetComponent<SafeGroundChecker>();
        if (safeGroundChecker)
        {
            safeGroundChecker.SetSafeGround(this);
        }
    }

    private void OnDrawGizmos()
    {
        if (safeGroundPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(safeGroundPosition.position, 0.5f);
        }
    }

}
