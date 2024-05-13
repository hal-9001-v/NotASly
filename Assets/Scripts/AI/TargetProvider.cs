using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProvider
{
    List<Transform> targets;

    public Transform LastTarget { get; private set; }

    public bool HasTargets => targets.Count > 0;

    public TargetProvider()
    {
        targets = new();
    }

    public Transform UpdateTarget()
    {
        if (targets.Count == 0)
        {
            LastTarget = null;
            return null;
        }

        LastTarget = targets[Random.Range(0, targets.Count)];

        return LastTarget;
    }

    public void AddTarget(Transform target)
    {
        if (targets.Contains(target) == false)
            targets.Add(target);
    }

    public void RemoveTarget(Transform target)
    {
        targets.Remove(target);
    }
}
