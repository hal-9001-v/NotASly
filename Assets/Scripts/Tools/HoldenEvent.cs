using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoldenEvent : MonoBehaviour
{
    [SerializeField][Range(0.1f, 5)] float time;
    [SerializeField][Range(0.1f, 5)] float resetTime;

    float holdTimeElapsedTime;
    float resetTimeElapsedTime;

    public UnityEvent OnHold;

    // Update is called once per frame
    void Update()
    {
        resetTimeElapsedTime += Time.deltaTime;
        if(resetTimeElapsedTime >= resetTime)
        {
            holdTimeElapsedTime = 0;
        }
    }

    public void Hold()
    {
        resetTimeElapsedTime = 0;
        holdTimeElapsedTime += Time.deltaTime;

        if (holdTimeElapsedTime >= time)
        {
            holdTimeElapsedTime = time;
            OnHold?.Invoke();
        }
    }
}
