using UnityEngine;
using UnityEngine.InputSystem;

public class WallSticker : MonoBehaviour, IPlayerState
{
	[SerializeField] Transform body;
	[SerializeField][Range(0, 5)] float checkDistance = 1.5f;
	[SerializeField][Range(0, 5)] float offset;

	[SerializeField][Range(0, 10)] float speed;

	WallStick currentWall;

	WallStick[] Walls => FindObjectsByType<WallStick>(FindObjectsSortMode.None);
	Mover Mover => GetComponent<Mover>();
	Player Player => GetComponent<Player>();

	public IPlayerState Next { get; private set; }

	float t;

	void Awake()
	{
		Exit();
	}

	public IPlayerState Check()
	{
		var closest = GetClosestPath();

		if (closest != null)
		{
			currentWall = (WallStick)closest;
			t = currentWall.Path.GetClosestT(transform.position);
			return this;
		}
		else
		{
			return null;
		}
	}

	public IPathInteractable GetClosestPath()
	{
		WallStick closest = null;
		float closestDistance = float.PositiveInfinity;
		foreach (var wall in Walls)
		{
			var distance = Vector3.Distance(wall.GetClosestPoint(transform.position), transform.position);
			if (distance < checkDistance && distance < closestDistance)
			{
				closest = wall;
				closestDistance = distance;
			}
		}

		return closest;
	}

	public Vector3 GetClosestPoint()
	{
		return currentWall.GetClosestPoint(transform.position);
	}

	void FixedUpdate()
	{
		Move(Player.Direction);
	}

	public void Move(Vector3 direction)
	{
		if (direction.magnitude > 0.1f)
			transform.position = currentWall.Path.GetPosition(t, out t, direction, speed * Time.deltaTime) + Vector3.up * offset;
		else
			transform.position = currentWall.Path.GetPosition(t) + Vector3.up * offset;

		body.rotation = Quaternion.LookRotation(Vector3.Cross(currentWall.Path.GetTangent(t), Vector3.up), Vector3.up);
	}

	private void OnJump()
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
		enabled = false;
		currentWall = null;
		Next = null;
	}
}
