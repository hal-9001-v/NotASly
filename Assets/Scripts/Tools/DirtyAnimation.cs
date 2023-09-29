using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyAnimation : MonoBehaviour
{
    [Header("Rotation")]
    public Vector3 rotationSpeed;
    [Header("Movement")]
    public Vector3 movementSpeed;

    public bool looped;
    [Range(1, 10)] public float loopTime;
    public float elapsedTime;
    float sign = 1;
    private void FixedUpdate()
    {
        
        if (looped)
        {
            elapsedTime += Time.fixedDeltaTime;
            if (elapsedTime > loopTime)
            {
                sign *= -1;
                elapsedTime = 0;
            }
        }

        transform.eulerAngles += rotationSpeed * Time.fixedDeltaTime * sign;
        transform.position += movementSpeed * Time.fixedDeltaTime * sign;
    }

}