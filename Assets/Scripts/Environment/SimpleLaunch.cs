using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleLaunch : MonoBehaviour
{
    [SerializeField] bool onStart;
    [SerializeField][Range(0, 1)] float verticalFactor = 0.75f;
    [SerializeField][Range(1, 10)] float speed = 5;
    [SerializeField] [Range(0, 1)] float speedVariation = 0.5f;

    Rigidbody Rigidbody => GetComponent<Rigidbody>();

    // Start is called before the first frame update
    void Start()
    {
        if (onStart)
        {
            Launch();
        }
    }

    [ContextMenu("Launch")]
    public void Launch()
    {
        var direction = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * Vector3.forward;
        direction = Vector3.Lerp(direction, Vector3.up, verticalFactor);
        var speed = this.speed * Random.Range(1 - speedVariation, 1 + speedVariation);

        Rigidbody.AddForce(direction * speed, ForceMode.VelocityChange);
    }
}
