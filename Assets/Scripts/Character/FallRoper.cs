using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRoper : MonoBehaviour, IPathFollower
{
    public bool Attatched => currentRope != null;

    public float CheckDistance => checkDistance;

    [SerializeField][Range(0, 10)] float checkDistance = 1.5f;
    [SerializeField][Range(0, 20)] float speed = 10f;

    FallRope currentRope;

    FallRope[] ropes => FindObjectsByType<FallRope>(FindObjectsSortMode.None);

    float t;

    public void Attach()
    {
        currentRope = GetClosestPath() as FallRope;
        if (currentRope)
        {
            t = currentRope.GetClosestT(transform.position);
        }
    }

    public bool Check()
    {
        var closest = GetClosestPath();
        return closest != null;
    }

    public void Dettach()
    {
        currentRope = null;
    }

    public IPathInteractable GetClosestPath()
    {
        FallRope closest = null;
        float closestDistance = float.MaxValue;
        foreach (var rope in ropes)
        {
            var distance = Vector3.Distance(transform.position, rope.GetClosestPoint(transform.position));
            if (distance < CheckDistance && distance < closestDistance)
            {
                closest = rope;
                closestDistance = distance;
            }
        }

        return closest;
    }

    public void Move(Vector2 input, Vector3 direction)
    {
        if (Attatched == false) return;

        t = currentRope.Path.GetTWithDisplacement(t, speed * Time.deltaTime);
        transform.position = currentRope.Path.GetPosition(t);

        if(t >= 1)
        {
            Dettach();
        }
    }

    public Vector3 GetClosestPoint()
    {
        return currentRope.GetClosestPoint(transform.position);
    }
}
