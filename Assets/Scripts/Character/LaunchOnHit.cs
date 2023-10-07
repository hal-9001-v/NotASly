using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Launchable))]
public class LaunchOnHit : MonoBehaviour
{
    Health Health => GetComponent<Health>();
    Launchable Launchable => GetComponent<Launchable>();

    [SerializeField][Range(0, 1)] float verticalFactor = 0.5f;
    [SerializeField][Range(1, 20)] float speed = 5;

    private void Awake()
    {
        Health.OnHurt += Launch;
    }

    void Launch(int damage, Vector3 origin)
    {
        var direction = transform.position - origin;
        direction.y = 0;
        direction.Normalize();

        direction = Vector3.Lerp(direction, Vector3.up, verticalFactor);
        Launchable.Launch(direction * speed);
    }
}
