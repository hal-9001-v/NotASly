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

    public TriggerInteractable Trigger => GetComponent<TriggerInteractable>();

    public Action<Health> OnHurt;
    public UnityEvent OnHurtEvent;

    public enum HurtType
    {
        Physical,
        Lightning,
        Fire,
    }

    [field: SerializeField] public HurtType Type { get; set; }

    public bool Apply
    {
        set
        {
            Trigger.Collider.enabled = value;
        }
    }

    private void Start()
    {
        Trigger.OnEnterAction += Hurt;
    }

    //Remember that either the Interactor or the HurtBox must have a rigidbody
    void Hurt(Interactor interactor)
    {
        Health health = interactor.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage, transform.position, Type);
            OnHurt?.Invoke(health);
            OnHurtEvent?.Invoke();
        }
    }

}
