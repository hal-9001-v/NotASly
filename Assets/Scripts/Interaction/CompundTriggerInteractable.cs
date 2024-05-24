using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompundTriggerInteractable : TriggerInteractable
{
    [Header("References")]
    [SerializeField] TriggerInteractable[] triggerInteractables;

    private void Awake()
    {
        if (GetComponent<Collider>())
        {
            Debug.LogWarning("This component in " + name + " should not use a trigger!");
        }

        for (int i = 0; i < triggerInteractables.Length; i++)
        {
            triggerInteractables[i].OnEnterAction += CheckEnter;
            triggerInteractables[i].OnExitAction += CheckExit;
        }
    }

    void CheckEnter(Interactor interactor)
    {
        int counter = 0;
        foreach (var trigger in triggerInteractables)
        {
            if (trigger.Interactors.Contains(interactor))
            {
                counter++;
                if (counter == 2)
                    return;
            }
        }

        if (debug)
        {
            Debug.Log("CompundTriggerInteractable: " + name + " enter triggered by " + interactor.name);
        }

        onEnterEvent.Invoke(interactor);
        OnEnterAction?.Invoke(interactor);
    }

    void CheckExit(Interactor interactor)
    {
        foreach (var trigger in triggerInteractables)
        {
            if (trigger.Interactors.Contains(interactor))
            {
                return;
            }
        }

        if (debug)
        {
            Debug.Log("CompundTriggerInteractable: " + name + " exit triggered by " + interactor.name);
        }

        onExitEvent.Invoke(interactor);
        OnExitAction?.Invoke(interactor);
    }

    [ContextMenu("Get rriggers from children")]
    void GetTriggersFromChildren()
    {
        
        triggerInteractables = GetComponentsInChildren<TriggerInteractable>().Where((trigger) => trigger != this).ToArray();
    }

}
