using System;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    public UnityEvent OnCollectEvent;
    public Action<Collector> OnCollectCallback;
    public bool AutoCollect = false;

    [SerializeField] TriggerInteractable triggerInteractable;

    private void Start()
    {
        if (AutoCollect)
        {
            var collector = FindAnyObjectByType<Collector>();
            if (collector != null)
                Collect(collector);
        }

        if (triggerInteractable == null)
            triggerInteractable = GetComponent<TriggerInteractable>();

        triggerInteractable.OnEnterAction += OnEnter;
    }

    void OnEnter(Interactor interactor)
    {
        if (OnCollectCallback == null) return;

        var collector = interactor.GetComponent<Collector>();
        if (collector != null)
            Collect(collector);
    }

    void Collect(Collector collector)
    {
        OnCollectCallback?.Invoke(collector);
        OnCollectEvent?.Invoke();
    }

}
