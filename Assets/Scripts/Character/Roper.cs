using System.Collections;
using System.Collections.Generic;
using UnityEditor.Splines;
using UnityEngine;
using UnityEngine.Splines;

public class Roper : MonoBehaviour
{
    [SerializeField][Range(1, 10)] float speed;
    [SerializeField][Range(0, 5)] float checkDistance;

    public Vector3 RopePosition => currentRope.GetPosition(t);

    Rope[] ropes => FindObjectsOfType<Rope>();

    Rope currentRope;
    float t;

    Vector3 direction;

    public bool Check()
    {
        foreach (var rope in ropes)
        {
            var closest = rope.GetClosestPoint(transform.position);
            Debug.DrawLine(transform.position, closest, Color.red);
            if (closest.y < transform.position.y)
            {
                if (Vector2.Distance(new Vector3(closest.x, closest.z), new Vector3(transform.position.x, transform.position.z)) < checkDistance)
                {
                    currentRope = rope;
                    t = rope.GetClosestT(transform.position);
                    return true;
                }
            }
        }

        return false;
    }

    public void Move(Vector3 direction)
    {
        this.direction = direction;
    }

    private void FixedUpdate()
    {
        if (currentRope == null)
            return;
        if (direction.magnitude > 0.1f)
            transform.position = currentRope.GetPosition(t, out t, direction, speed * Time.deltaTime);
        else
            transform.position = currentRope.GetPosition(t);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkDistance);
    }
}
