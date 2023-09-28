using System;
using UnityEngine;

public abstract class BaseSensor : MonoBehaviour
{
    public Action<BaseSenseTrigger> OnSense;

    protected abstract void Check();



}
