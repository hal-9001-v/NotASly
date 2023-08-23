using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerInteractable : MonoBehaviour
{
    [SerializeField] UnityEvent<Interactor> onEnterEvent;
    [SerializeField] UnityEvent<Interactor> onExitEvent;

    public Action<Interactor> OnEnterAction;
    public Action<Interactor> OnExitAction;

    [SerializeField] bool debug;


    private void OnTriggerEnter(Collider other)
    {
        Interactor interactor = other.GetComponent<Interactor>();
        if (interactor != null)
        {
            onEnterEvent.Invoke(interactor);
            OnEnterAction?.Invoke(interactor);

            if (debug)
            {
                Debug.Log("TriggerInteractable: " + name + " enter triggered by " + interactor.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactor interactor = other.GetComponent<Interactor>();
        if (interactor != null)
        {
            onExitEvent.Invoke(interactor);
            OnExitAction?.Invoke(interactor);

            if (debug)
            {
                Debug.Log("TriggerInteractable: " + name + " exit triggered by " + interactor.name);
            }
        }
    }
}
