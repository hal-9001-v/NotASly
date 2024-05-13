using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSenseTrigger : MonoBehaviour
{
    public Action<BaseSensor> OnSensed;
    public bool CanBeSensed = true;
}
