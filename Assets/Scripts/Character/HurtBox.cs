using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TriggerInteractable))]
public class HurtBox : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField][Range(0, 10)] int damage;

    TriggerInteractable trigger => GetComponent<TriggerInteractable>();

    public Action<Health> OnHurt;
    public UnityEvent OnHurtEvent;

    public bool Apply
    {
        set
        {
            trigger.Collider.enabled = value;
        }
    }

    private void Start()
    {
        trigger.OnEnterAction += Hurt;
    }

    //Remember that either the Interactor or the HurtBox must have a rigidbody
    void Hurt(Interactor interactor)
    {
        Health health = interactor.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
            OnHurt?.Invoke(health);
            OnHurtEvent?.Invoke();
        }
    }

}
