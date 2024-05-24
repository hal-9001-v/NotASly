using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniTrigger : BaseSenseTrigger
{
    public void RemoveFromOmniSensors()
    {
        foreach (var sensor in FindObjectsByType<OmniSensor>(FindObjectsSortMode.None))
        {
            sensor.RemoveTarget(this);
        }

    }
}
