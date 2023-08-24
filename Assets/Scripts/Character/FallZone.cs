using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerInteractable))]
public class FallZone : MonoBehaviour
{
    void Start()
    {
        var triggerInteractable = GetComponent<TriggerInteractable>();
        triggerInteractable.OnEnterAction += Fall;
    }

    void Fall(Interactor interactor)
    {
        var environmentInfo = interactor.GetComponent<EnvironmentInfo>();
        if (environmentInfo != null)
            environmentInfo.Fall();

    }
}
