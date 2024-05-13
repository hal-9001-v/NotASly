using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerInteractable))]
public class Healer : MonoBehaviour
{
    public int healAmount = 1;
    Collectable Collectable => GetComponent<Collectable>();

    // Start is called before the first frame update
    void Start()
    {
        Collectable.OnCollectCallback += Heal;
    }

    void Heal(Collector collector)
    {
        var health = collector.GetComponent<Health>();
        if (health != null)
        {
            health.Heal(healAmount);
        }
    }
}
