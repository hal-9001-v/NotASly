using System;
using UnityEngine;

public abstract class BaseSensor : MonoBehaviour
{
    public Action<BaseSenseTrigger> OnSense;
    public Action<BaseSenseTrigger> OnLost;


    protected abstract void Check();


    private void Update()
    {
        Check();
    }

}
