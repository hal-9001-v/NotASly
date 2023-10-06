using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerInteractable))]
public class Healer : MonoBehaviour
{
    public int healAmount = 1;
    TriggerInteractable TriggerInteractable => GetComponent<TriggerInteractable>();

    // Start is called before the first frame update
    void Start()
    {
        TriggerInteractable.OnEnterAction += Heal;
    }

    void Heal(Interactor interactor)
    {
        var health = interactor.GetComponent<Health>();
        if (health != null)
        {
            health.Heal(healAmount);
        }
    }
}
