using System.Collections.Generic;
using UnityEngine;

public class Roper : MonoBehaviour, IPlayerState, ISafe
{
	[SerializeField][Range(0, 2)] float yOffset;
	[SerializeField][Range(1, 10)] float speed;
	[SerializeField][Range(0, 5)] float checkDistance;

	public Vector3 RopePosition => currentRope.Path.GetPosition(t);
	Mover Mover => GetComponent<Mover>();
	Rope[] Ropes => FindObjectsByType<Rope>(FindObjectsSortMode.None);

	Player Player => GetComponent<Player>();

	Aligner Aligner => GetComponent<Aligner>();


	public float CheckDistance => checkDistance;

	public float SafeT;

	public IPlayerState Next { get; private set; }

	Rope currentRope;
	float t;

	private void Awake()
	{
		Exit();
	}

	public IPlayerState Check()
	{
		var closest = GetClosestPath();
		if (closest != null)
		{
			currentRope = (Rope)GetClosestPath();
			t = currentRope.Path.GetClosestT(transform.position);
			return Aligner.Align(closest.GetClosestPoint(transform.position), this);
		}
		else
		{
			return null;
		}
	}

	private void Update()
	{
		Move(Player.Direction);
	}

	public void Move(Vector3 direction)
	{
		if (direction.magnitude > 0.1f)
		{
			var movement = transform.position;
			transform.position = currentRope.Path.GetPosition(t, out t, direction, speed * Time.deltaTime) + Vector3.up * yOffset;
			movement = transform.position - movement;

			movement.y = 0;
			movement.Normalize();

			Mover.Steer(movement);

		}
		else
			transform.position = currentRope.Path.GetPosition(t) + Vector3.up * yOffset;

		SafeT = t;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, checkDistance);
	}

	public IPathInteractable GetClosestPath()
	{
		Rope closest = null;
		float closesDistance = float.MaxValue;
		foreach (var rope in Ropes)
		{
			var distance = Vector3.Distance(transform.position, rope.GetClosestPoint(transform.position));
			if (distance < checkDistance && distance < closesDistance)
			{
				closesDistance = distance;
				closest = rope;
			}
		}

		return closest;
	}

	public Vector3 GetClosestPoint()
	{
		return currentRope.GetClosestPoint(transform.position);
	}

	void OnJump()
	{
		if (enabled)
		{
			Mover.JumpWithInertia();
			Next = Mover;
		}
	}

	public void Enter()
	{
		enabled = true;
	}

	public void Exit()
	{
		enabled = false;
		Next = null;
	}

	public IPlayerState BackToSafe(Player player)
	{
		t = SafeT;
		return Aligner.Align(currentRope.Path.GetPosition(t), this);
	}

}
