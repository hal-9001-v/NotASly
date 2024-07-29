using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Destroyer))]
public class SpawnBullet : MonoBehaviour
{

    [SerializeField][Range(0, 100)] float speed = 10;

    [SerializeField] HurtBox hurtBox;
    Rigidbody Rigidbody => GetComponent<Rigidbody>();
    Destroyer Destroyer => GetComponent<Destroyer>();

    Transform Target
    {
        get
        {
            var player = FindAnyObjectByType<Player>();
            if (player != null)
            {
                return player.transform;
            }
            return null;

        }
    }

    Vector3 direction;

    private void Awake()
    {
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }

        Rigidbody.isKinematic = true;

        direction = (Target.position - transform.position).normalized;
        hurtBox.Trigger.OnAnyEnterAction += Impact;
    }

    private void FixedUpdate()
    {
        transform.position += direction * Time.fixedDeltaTime * speed;
    }

    void Impact()
    {
        Destroyer.Destroy();
    }

}
