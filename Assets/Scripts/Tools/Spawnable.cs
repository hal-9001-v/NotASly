using System;
using UnityEngine;
using UnityEngine.Events;

public class Spawnable : MonoBehaviour
{
    public Action OnSpawn;
    public UnityEvent OnSpawnEvent;

    public void Spawn()
    {
        OnSpawn?.Invoke();
        OnSpawnEvent?.Invoke();
    }
}
