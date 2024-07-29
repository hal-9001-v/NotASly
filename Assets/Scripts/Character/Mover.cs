using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour, IPlayerState
{
	[Header("References")]
	[SerializeField] Transform rotatingPivot;

	[Header("Movement")]
	[SerializeField] float speed = 6f;
	[SerializeField] float acceleration = 10f;
	[SerializeField] float airControl = 0.1f;
	[SerializeField] float jumpHeight = 5;
	[SerializeField] float doubleJumpHeight = 5;
	[SerializeField] float gravity = -20;

	[SerializeField][Range(0, 720)] float rotatingSpeed;

	float currentSpeed;
	Vector3 velocity;

	Vector3 movingDirection;

	bool doubleJumpReady;
	Player Player => GetComponent<Player>();
	CharacterController Controller => GetComponent<CharacterController>();
	GroundCheck GroundCheck => GetComponent<GroundCheck>();
	PlayerAnimator PlayerAnimator => GetComponentInChildren<PlayerAnimator>();
	PickPocketer PickPocketer => GetComponent<PickPocketer>();
	Interactor Interactor => GetComponent<Interactor>();
	CameraSelector CameraSelector => FindAnyObjectByType<CameraSelector>();
	Meleer Meleer => GetComponent<Meleer>();
	public IPlayerState Next { get; private set; }

	IPlayerState[] interactionTransitions;

	public IPlayerState Check() => this;

	private void Awake()
	{
		interactionTransitions = new IPlayerState[]
		{
			GetComponent <WallSticker>(),
			GetComponent<Venter>(),
			GetComponent<Roper>(),
			GetComponent<Piper>(),
			GetComponent<Slider>(),
			GetComponent<SharpPointer>(),
			GetComponent<Gripper>(),
		};

		Exit();
	}

	private void FixedUpdate()
	{
		Move(Player.Direction);
	}

	public void Move(Vector3 direction)
	{
		velocity.y += gravity * Time.deltaTime;
		GroundCheck.SetYSpeed(velocity.y);
		Steer(direction);

		if (GroundCheck.IsGrounded)
		{
			velocity.y = 0f;

			if (direction.magnitude > 0.1f)
			{
				currentSpeed += acceleration * Time.fixedDeltaTime;
				if (currentSpeed > speed)
				{
					currentSpeed = speed;
				}

				velocity = movingDirection * currentSpeed;
			}
			else
			{
				velocity = Vector3.zero;

			}
		}
		else
		{
			var desiredVelocity = speed * movingDirection;
			desiredVelocity.y = velocity.y;

			velocity = Vector3.Lerp(velocity, desiredVelocity, airControl * Time.deltaTime);
		}

		//Just to avoid annoying messages from unity in the console :D
		if (Controller.enabled)
			Controller.Move(velocity * Time.deltaTime);
	}

	public void Steer(Vector3 direction, bool insta = false)
	{
		if (direction.magnitude < 0.1f)
		{
			movingDirection = Vector3.zero;
			return;
		}

		if (insta)
		{
			rotatingPivot.rotation = Quaternion.LookRotation(direction, Vector3.up);
			return;
		}

		var targetRotation = Quaternion.LookRotation(direction, Vector3.up);

		var angle = Quaternion.Angle(rotatingPivot.rotation, targetRotation);

		rotatingPivot.rotation = Quaternion.Lerp(rotatingPivot.rotation, targetRotation, Time.fixedDeltaTime * rotatingSpeed / angle);
		movingDirection = rotatingPivot.forward;
	}

	public void JumpWithInertia()
	{
		JumpWithInertia(Player.Direction);
	}

	public void JumpWithInertia(Vector3 direction)
	{
		Jump();
		velocity += direction * speed;
	}

	public void Jump()
	{
		Jump(jumpHeight);
	}

	public void DoubleJump()
	{
		Jump(doubleJumpHeight);
	}

	public void Jump(float height)
	{
		velocity.y = Mathf.Sqrt(height * -2f * gravity);
		GroundCheck.SetYSpeed(velocity.y);
	}

	void OnHit(InputValue value)
	{
		if(value.isPressed == false) return;

		if (enabled)
		{
			Meleer.Hit();
		}
	}

	void OnInteract(InputValue value)
	{
		if (value.isPressed == false) return;

		if (enabled && Next == null)
		{
			foreach (var transition in interactionTransitions)
			{
				Next = transition.Check();
				if (Next != null)
					break;

			}
		}
	}

	public void Enter()
	{
		CameraSelector.UseFollowCamera();
		enabled = true;
		doubleJumpReady = true;
	}

	public void Exit()
	{
		velocity = Vector3.zero;
		enabled = false;
		Next = null;
	}

	void OnJump()
	{
		if (GroundCheck.IsGrounded)
		{
			doubleJumpReady = true;
			Jump();
			PlayerAnimator.Jump();
		}
		else if (doubleJumpReady)
		{
			doubleJumpReady = false;
			DoubleJump();
			PlayerAnimator.Jump();
		}
	}

}
