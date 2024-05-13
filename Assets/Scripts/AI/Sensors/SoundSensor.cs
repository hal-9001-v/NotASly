using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSensor : BaseSensor
{
    [SerializeField][Range(1, 100)] float soundRadius = 10f;

    SoundTrigger[] SoundTriggers => FindObjectsOfType<SoundTrigger>();

    List<SoundTrigger> soundTriggers;

    public SoundTrigger LastSensed { get; private set; }

    public bool IsSensing => soundTriggers.Count > 0;

    private void Awake()
    {
        soundTriggers = new List<SoundTrigger>();
    }

    protected override void Check()
    {
        foreach (SoundTrigger soundTrigger in SoundTriggers)
        {
            if (soundTrigger.CanBeSensed)
            {
                if (Vector3.Distance(transform.position, soundTrigger.transform.position) < soundRadius + soundTrigger.SoundRadius)
                {
                    if (soundTriggers.Contains(soundTrigger) == false)
                    {
                        LastSensed = soundTrigger;
                        soundTriggers.Add(soundTrigger);
                        OnSense?.Invoke(soundTrigger);
                    }
                }

            }
        }

        for (int i = soundTriggers.Count - 1; i >= 0; i--)
        {
            if (soundTriggers[i] == null || soundTriggers[i].CanBeSensed == false)
            {
                OnLost?.Invoke(soundTriggers[i]);
                soundTriggers.RemoveAt(i);
            }

        }
    }
}
