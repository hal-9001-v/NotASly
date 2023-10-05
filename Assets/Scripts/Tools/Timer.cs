using System;
using UnityEngine;

[Serializable]
public class Timer
{
    [SerializeField] float time;
    float elapsedTime;

    public float NormalizedTime => Mathf.Clamp01(elapsedTime / time);

    public bool IsFinished => elapsedTime >= time;

    public Timer(float time)
    {
        this.time = time;
    }

    public bool UpdateTimer()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= time)
        {
            return true;
        }
        return false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0;
    }

    public void ResetTimer(float time)
    {
        this.time = time;
        elapsedTime = 0;
    }


}
