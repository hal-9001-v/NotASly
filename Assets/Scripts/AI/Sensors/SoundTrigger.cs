using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : BaseSenseTrigger
{
    [SerializeField] float soundRadius = 10f;
    public float SoundRadius => soundRadius;
    [SerializeField] Timer duration;

    private void Update()
    {
        if (CanBeSensed)
        {
            if (duration.UpdateTimer())
            {
                CanBeSensed = false;
            }
        }
    }

    public void Noise()
    {
        CanBeSensed = true;
        duration.ResetTimer();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, soundRadius);
    }

}
