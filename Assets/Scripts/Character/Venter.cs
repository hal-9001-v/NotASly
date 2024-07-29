using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Venter : MonoBehaviour, IPlayerState
{
	[Header("Settings")]
	[SerializeField] float speed = 6f;
	[SerializeField] float gravity = 20f;
	[SerializeField][Range(0, 1)] float ventControllerHeight = 0.5f;
	[SerializeField][Range(0, 1)] float ventControllerRadius = 0.5f;
	[SerializeField] Transform body;
	[SerializeField] Transform cameraLookAt;

	bool isInside;

	CharacterController CharacterController => GetComponent<CharacterController>();
	Mover Mover => GetComponent<Mover>();
	SightSensorTrigger SightTrigger => GetComponent<SightSensorTrigger>();
	CameraSelector CameraSelector => FindAnyObjectByType<CameraSelector>();
	Player Player => GetComponent<Player>();

	public IPlayerState Next { get; private set; }

	float defaultControllerHeight;
	float defaultControllerRadius;

	private void Awake()
	{
		defaultControllerHeight = CharacterController.height;
		defaultControllerRadius = CharacterController.radius;
		Exit();
	}

	public IPlayerState Check()
	{
		if (isInside)
		{
			return this;
		}

		return null;
	}

	private void Update()
	{
		Move(Player.Direction);

		if (Check() == null)
		{
			Next = Mover;
		}
	}

	public void Move(Vector3 direction)
	{
		var velocity = direction * speed + Vector3.down * gravity;
		CharacterController.Move(velocity * Time.deltaTime);

		var lookAt = cameraLookAt.forward;
		lookAt.y = 0;
		lookAt.Normalize();
		body.rotation = Quaternion.LookRotation(lookAt, Vector3.up);
	}

	public void EnterVent() => isInside = true;

	public void ExitVent() => isInside = false;

	public void Enter()
	{
		CharacterController.height = ventControllerHeight;
		CharacterController.radius = ventControllerRadius;
		SightTrigger.CanBeSensed = false;
		CameraSelector.UseVentCamera();

		enabled = true;
	}

	public void Exit()
	{
		CharacterController.height = defaultControllerHeight;
		CharacterController.radius = defaultControllerRadius;
		SightTrigger.CanBeSensed = true;

		enabled = false;
		Next = null;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, ventControllerHeight);
	}

}
