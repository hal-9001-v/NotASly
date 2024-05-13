using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Venter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float speed = 6f;
    [SerializeField] float gravity = 20f;
    [SerializeField][Range(0, 1)] float ventControllerHeight = 0.5f;
    [SerializeField][Range(0, 1)] float ventControllerRadius = 0.5f;

    bool isInside;

    CharacterController CharacterController => GetComponent<CharacterController>();

    SightSensorTrigger SightTrigger => GetComponent<SightSensorTrigger>();

    float defaultControllerHeight;
    float defaultControllerRadius;

    private void Awake()
    {
        defaultControllerHeight = CharacterController.height;
        defaultControllerRadius = CharacterController.radius;
    }

    public bool Check()
    {
        if (isInside)
        {
            return true;
        }

        return false;
    }

    public void Attach()
    {
        CharacterController.height = ventControllerHeight;
        CharacterController.radius = ventControllerRadius;
    }

    public void Release()
    {
        CharacterController.height = defaultControllerHeight;
        CharacterController.radius = defaultControllerRadius;
    }

    public void Move(Vector3 direction)
    {
        var velocity = direction * speed + Vector3.down * gravity;
        CharacterController.Move(velocity * Time.deltaTime);
    }

    public void EnterVent()
    {
        isInside = true;
        SightTrigger.CanBeSensed = false;
    }

    public void ExitVent()
    {
        isInside = false;
        SightTrigger.CanBeSensed = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ventControllerHeight);
    }
}
