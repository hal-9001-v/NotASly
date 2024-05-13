using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SightSensor : BaseSensor
{
    [Header("Settings")]
    [SerializeField][Range(1, 50)] private float range = 10;
    [SerializeField][Range(1, 180)] private float angle = 45;

    SightSensorTrigger[] SightSensorTriggers => FindObjectsOfType<SightSensorTrigger>().Where((trigger) => trigger.CanBeSensed).ToArray();

    public List<SightSensorTrigger> TriggersOnSight { get; private set; }
    public SightSensorTrigger LastSeen { get; private set; }


    public void Awake()
    {
        TriggersOnSight = new List<SightSensorTrigger>();
    }

    protected override void Check()
    {
        foreach (var trigger in SightSensorTriggers)
        {
            if (trigger == null)
                continue;

            if (trigger.gameObject == gameObject)
                continue;

            if (IsInSight(trigger))
            {
                LastSeen = trigger;
                if (!TriggersOnSight.Contains(trigger))
                {
                    TriggersOnSight.Add(trigger);
                    OnSense?.Invoke(trigger);
                }
            }
            else
            {
                if (TriggersOnSight.Contains(trigger))
                {
                    TriggersOnSight.Remove(trigger);
                    OnLost?.Invoke(trigger);
                }
            }
        }
    }


    private bool IsInSight(SightSensorTrigger trigger)
    {
        Vector3 direction = trigger.transform.position - transform.position;
        float angleToTrigger = Vector3.Angle(transform.forward, direction);

        if (direction.magnitude > range)
        {
            return false;
        }

        if (angleToTrigger <= angle)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, range))
            {
                if (hit.collider.gameObject == trigger.gameObject)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + transform.forward * range);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle, 0) * transform.forward * range);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle, 0) * transform.forward * range);
    }
}
