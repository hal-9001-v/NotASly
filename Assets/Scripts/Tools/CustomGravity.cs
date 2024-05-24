using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    [SerializeField][Range(1, 100)] float gravity;
    Rigidbody Rigidbody => GetComponent<Rigidbody>();

    private void Awake()
    {
        Rigidbody.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody.AddForce(Vector3.down * gravity * Rigidbody.mass);
    }
}
