using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Interactor))]
public class TouchTrigger : BaseSenseTrigger
{
    private void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

}
