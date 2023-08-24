using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentInfo : MonoBehaviour
{
    [SerializeField] UnityEvent onFallEvent;
    public Action OnFallAction;

    public void Fall()
    {
        onFallEvent.Invoke();
        OnFallAction?.Invoke();
    }
}
