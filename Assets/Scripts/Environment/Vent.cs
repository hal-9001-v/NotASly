using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    [SerializeField] TriggerInteractable[] entries;
    [SerializeField] TriggerInteractable inside;

    private void Start()
    {
        inside.OnEnterAction += CheckInsideIn;
        inside.OnExitAction += CheckInsideOut;

        foreach (var entry in entries)
        {
            entry.OnEnterAction += CheckEntryIn;
            entry.OnExitAction += CheckEntryOut;
        }
    }

    void CheckInsideIn(Interactor interactor)
    {
        var venter = interactor.GetComponent<Venter>();
        if (venter)
        {
            venter.IsInside = true;
        }
    }

    void CheckInsideOut(Interactor interactor)
    {
        var venter = interactor.GetComponent<Venter>();
        if (venter)
        {
            venter.IsInside = false;
        }
    }

    void CheckEntryIn(Interactor interactor)
    {
        var venter = interactor.GetComponent<Venter>();
        if (venter)
        {
            venter.IsInEntry = true;
        }
    }

    void CheckEntryOut(Interactor interactor)
    {
        var venter = interactor.GetComponent<Venter>();
        if (venter)
        {
            venter.IsInEntry = false;
        }
    }
}
