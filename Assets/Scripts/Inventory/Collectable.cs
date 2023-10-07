using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TriggerInteractable))]
public class Collectable : MonoBehaviour
{
    public UnityEvent OnCollectEvent;
    public Action<Collector> OnCollectCallback;

    TriggerInteractable TriggerInteractable => GetComponent<TriggerInteractable>();

    private void Start()
    {
        TriggerInteractable.OnEnterAction += OnEnter;
    }

    void OnEnter(Interactor interactor)
    {
        var collector = interactor.GetComponent<Collector>();
        if (collector != null)
        {
            OnCollectCallback.Invoke(collector);
            OnCollectEvent.Invoke();
        }
    }


}
