using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Aligner : MonoBehaviour, IPlayerState
{
	[SerializeField] CharacterController characterController;
	[SerializeField] float gravity = 20;
	[SerializeField] float minJumpHeight = 2;
	[SerializeField] float alignSpeed = 10f;

	Vector3 startPosition;
	Vector3 endPosition;

	float elapsedTime;
	float time = -1;

	float yPos;
	float yVelocity;

	Action callbackAction;

	bool aligning;

	public IPlayerState Next { get; private set; }

	private void Awake()
	{
		aligning = false;
	}

	public IPlayerState Align(Vector3 position, IPlayerState next)
	{
		if (aligning)
		{
			return this;
		}

		Align(position, () => Next = next);
		return this;
	}

	public void Align(Vector3 position, Action callback)
	{
		callbackAction = callback;

		startPosition = transform.position;
		endPosition = position;

		var distance = startPosition.HorizontalDitance(endPosition);
		if (distance < 0.1f)
		{
			time = 0.1f;
		}
		else
		{
			time = distance / alignSpeed;
		}

		float a = endPosition.y - startPosition.y;
		float b = Mathf.Pow(time * 0.5f, 2) * gravity * 0.5f;

		float jumpHeight = a + b;
		if (jumpHeight < minJumpHeight)
		{
			jumpHeight = minJumpHeight;
		}

		yVelocity = Mathf.Sqrt(2 * gravity * (jumpHeight));
		yPos = startPosition.y;

		elapsedTime = 0;
		aligning = true;
	}

	private void FixedUpdate()
	{
		if (aligning)
		{
			if (elapsedTime < time)
			{
				elapsedTime += Time.fixedDeltaTime;
			}

			var pos = Vector3.Lerp(startPosition, endPosition, elapsedTime / time);

			yVelocity -= gravity * Time.fixedDeltaTime;
			yPos += yVelocity * Time.fixedDeltaTime;
			pos.y = yPos;

			transform.position = pos;

			if (transform.position.HorizontalDitance(endPosition) < 0.1f && yPos <= endPosition.y)
			{
				callbackAction?.Invoke();
				aligning = false;
			}
		}
	}
	public void Enter()
	{

	}

	public void Exit()
	{
		Next = null;
	}

	public IPlayerState Check()
	{
		throw new NotImplementedException();
	}
}
