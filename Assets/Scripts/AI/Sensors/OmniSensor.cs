using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniSensor : BaseSensor
{

    public List<OmniTrigger> Triggers { get; private set; }

    public bool IsSensing => Triggers.Count > 0;

    private void Awake()
    {
        Triggers = new List<OmniTrigger>();
    }

    protected override void Check()
    {
        
    }

    public void AddTarget(OmniTrigger target)
    {
        if (Triggers.Contains(target) == false)
        {
            Triggers.Add(target);
        }
    }

    public void RemoveTarget(OmniTrigger target)
    {
        Triggers.Remove(target);

    }
}
