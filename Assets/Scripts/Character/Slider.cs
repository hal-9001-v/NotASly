using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour, IPlayerState
{
    public bool Attatched => currentRope != null;

    public float CheckDistance => checkDistance;

    [SerializeField][Range(0, 10)] float checkDistance = 1.5f;
    [SerializeField][Range(0, 20)] float speed = 10f;

    FallRope currentRope;

    FallRope[] Ropes => FindObjectsByType<FallRope>(FindObjectsSortMode.None);
    Player Player => GetComponent<Player>();

	public IPlayerState Next => throw new System.NotImplementedException();

	float t;

    public void Attach()
    {
        currentRope = GetClosestPath() as FallRope;
        if (currentRope)
        {
            t = currentRope.GetClosestT(transform.position);
        }
    }

    public IPlayerState Check()
    {
        var closest = GetClosestPath();
        if (closest != null)
        {
			return this;
		}
		else
        {
			return null;
		}
    }

    public void Dettach()
    {
        currentRope = null;
    }

    public IPathInteractable GetClosestPath()
    {
        FallRope closest = null;
        float closestDistance = float.MaxValue;
        foreach (var rope in Ropes)
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

    public void Move(Vector2 input, bool pressing, Vector3 direction)
    {
        if (Attatched == false) return;

        t = currentRope.Path.GetTWithDisplacement(t, speed * Time.deltaTime);
        transform.position = currentRope.Path.GetPosition(t);

        if (t >= 1)
        {
            Dettach();
        }
    }

    public Vector3 GetClosestPoint()
    {
        return currentRope.GetClosestPoint(transform.position);
    }

	public void Enter()
	{
		throw new System.NotImplementedException();
	}

	public void Exit()
	{
		throw new System.NotImplementedException();
	}
}
