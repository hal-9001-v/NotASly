using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerInteractable))]
public class TouchSensor : BaseSensor
{
    TriggerInteractable TriggerInteractable => GetComponent<TriggerInteractable>();

    List<TouchTrigger> touchTriggers;

    public bool IsTouching => touchTriggers.Count > 0;

    protected override void Check()
    {

    }

    void Start()
    {
        touchTriggers = new List<TouchTrigger>();

        TriggerInteractable.OnEnterAction += (c) =>
        {
            var touch = c.GetComponent<TouchTrigger>();
            if (touch != null)
            {
                if (!touchTriggers.Contains(touch))
                {
                    touchTriggers.Add(touch);
                    OnSense?.Invoke(touch);
                }
            }
        };

        TriggerInteractable.OnExitAction += (c) =>
        {
            var touch = c.GetComponent<TouchTrigger>();
            if (touch != null)
            {
                if (touchTriggers.Contains(touch))
                {
                    touchTriggers.Remove(touch);
                    OnLost?.Invoke(touch);
                }
            }
        };
    }

}
