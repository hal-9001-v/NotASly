using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HurtBox))]
public class HurtSafeGround : MonoBehaviour
{
    HurtBox HurtBox => GetComponent<HurtBox>();

    private void Start()
    {
        HurtBox.OnHurt += OnHurt;
    }

    void OnHurt(Health health)
    {
        var safeGround = health.GetComponent<SafeGroundChecker>();
        if (safeGround)
        {
            safeGround.GetToSafeGround();
        }
    }
}
