using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventHolder : MonoBehaviour
{
    [SerializeField] EventHolder[] events;

    public void Invoke(int index)
    {
        if (index < 0 || index >= events.Length)
        {
            Debug.LogError("Index out of range");
            return;
        }

        events[index].Invoke();
    }

    public void Invoke(string name)
    {
        var namedEvent = events.FirstOrDefault(e => e.Name == name);

        if(namedEvent == null)
        {
            Debug.LogError("Event not found");
            return;
        }

        namedEvent.Invoke();
    }

    [Serializable]
    class EventHolder
    {
        public string Name;
        public UnityEvent Event;

        public void Invoke()
        {
            Event.Invoke();
        }

    }
}
