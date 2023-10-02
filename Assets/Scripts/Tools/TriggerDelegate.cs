using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerDelegate : MonoBehaviour
{
    Action<Collider> OnEnterAction;

}
