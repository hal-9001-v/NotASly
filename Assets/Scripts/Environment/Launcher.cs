using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerInteractable))]
public class Launcher : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField][Range(0, 20)] float launchHeight = 5;

    private void Start()
    {
        GetComponent<TriggerInteractable>().OnEnterAction += Launch;
    }

    public void Launch(Interactor interactor)
    {
        var mover = interactor.GetComponent<Mover>();
        if (mover)
        {
            mover.Jump(launchHeight);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * launchHeight);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * launchHeight, 0.3f);
    }
}
