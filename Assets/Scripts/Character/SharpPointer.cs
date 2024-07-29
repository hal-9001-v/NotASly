using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SharpPointer : MonoBehaviour, IPlayerState, ISafe
{
	[SerializeField][Range(0, 5)] float checkRadius = 1f;
	SharpPoint[] SharpPoints => FindObjectsByType<SharpPoint>(FindObjectsSortMode.None);
	Aligner Aligner => GetComponent<Aligner>();

	Mover Mover => GetComponent<Mover>();

	Player Player => GetComponent<Player>();

	public IPlayerState Next { get; private set; }

	SharpPoint safeSharpPoint;

	public IPlayerState Check()
	{
		var closest = GetClosest();
		if (closest)
		{
			if (transform.position.HorizontalDitance(closest.Point) < checkRadius)
			{
				safeSharpPoint = closest;
				return Aligner.Align(closest.Point, this);
			}
		}

		return null;
	}

	void Awake()
	{
		Exit();
	}

	private void FixedUpdate()
	{
		Mover.Steer(Player.Direction);
	}

	public IPlayerState Attach()
	{
		var closest = GetClosest();
		Aligner.Align(closest.Point, this);
		return Aligner;
	}

	public SharpPoint GetClosest()
	{
		SharpPoint closest = null;
		float closestDistance = float.MaxValue;
		foreach (var point in SharpPoints)
		{

			if (point.Point.y < transform.position.y && transform.position.HorizontalDitance(point.Point) < closestDistance)
			{
				closest = point;
				closestDistance = Vector3.Distance(transform.position, point.Point);
			}
		}

		return closest;
	}

	void OnJump()
	{
		if (enabled)
		{
			Mover.JumpWithInertia(Player.Direction);
			Next = Mover;
		}
	}

	public void Enter()
	{
		enabled = true;
	}

	public void Exit()
	{
		Next = null;
		enabled = false;
	}

	public IPlayerState BackToSafe(Player player)
	{
		return Aligner.Align(safeSharpPoint.Point, this);
	}
}
