using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    [SerializeField] TriggerInteractable inside;

    private void Start()
    {
        inside.OnEnterAction += CheckInsideIn;
        inside.OnExitAction += CheckInsideOut;

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
}
