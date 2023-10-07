using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DirtyAnimation : MonoBehaviour
{
    [Header("Rotation")]
    public Vector3 rotationSpeed;
    [Header("Movement")]
    public Vector3 movementSpeed;
    [Header("Scale")]
    public Vector3 scaleSpeed;

    public bool looped;
    [Range(0, 10)] public float loopTime;
    public float elapsedTime;
    float sign = 1;

    public UnityEvent OnEndLoop;

    private void FixedUpdate()
    {
        
        if (looped)
        {
            elapsedTime += Time.fixedDeltaTime;
            if (elapsedTime > loopTime)
            {
                sign *= -1;
                elapsedTime = 0;
                OnEndLoop?.Invoke();
            }
        }

        transform.eulerAngles += rotationSpeed * Time.fixedDeltaTime * sign;
        transform.position += movementSpeed * Time.fixedDeltaTime * sign;
        transform.localScale += scaleSpeed * Time.fixedDeltaTime * sign;
    }

    public void ResetTimer()
    {
        elapsedTime = 0;
    }

}
