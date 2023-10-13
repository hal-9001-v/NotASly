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

    public bool IsInside;

    CharacterController CharacterController => GetComponent<CharacterController>();
    float defaultControllerHeight;
    float defaultControllerRadius;

    private void Awake()
    {
        defaultControllerHeight = CharacterController.height;
        defaultControllerRadius = CharacterController.radius;
    }

    public bool Check()
    {
        if (IsInside)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ventControllerHeight);
    }
}
