using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Meleer))]
[RequireComponent(typeof(Piper))]
[RequireComponent(typeof(SharpPointer))]
[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(WallSticker))]
[RequireComponent(typeof(Gripper))]
[RequireComponent(typeof(EnvironmentInfo))]
[RequireComponent(typeof(SafeGroundChecker))]
[RequireComponent(typeof(Roper))]
[RequireComponent(typeof(Aligner))]
[RequireComponent(typeof(Venter))]
public class Player : MonoBehaviour
{
	[SerializeField] Transform cameraFollow;

	[SerializeField] float cameraSpeed = 5f;
	[SerializeField] Timer interactionTimer;
	[SerializeField] Timer fallTimer;

	public ISafe safe;
	SafeGround safeGround;

	Health Health => GetComponent<Health>();
	Launchable Launchable => GetComponent<Launchable>();
	Rigidbody Rigidbody => GetComponent<Rigidbody>();
	CharacterController CharacterController => GetComponent<CharacterController>();
	CameraSelector CameraSelector => FindAnyObjectByType<CameraSelector>();
	PlayerHud PlayerHud => FindAnyObjectByType<PlayerHud>();

	EnvironmentInfo EnvironmentInfo => GetComponent<EnvironmentInfo>();
	Aligner Aligner => GetComponent<Aligner>();

	GameOver GameOver => FindAnyObjectByType<GameOver>();

	Pause Pause => FindAnyObjectByType<Pause>();

	IPlayerState StartingState => GetComponent<Mover>();

	[SerializeField] IPlayerState currentState;
	public Vector3 Direction => CameraRotation * new Vector3(input.x, 0, input.y);
	public Quaternion CameraRotation { get; private set; }
	public bool InputInteraction { get; private set; }

	Vector2 cameraInput;
	Vector2 input;
	bool IsPressingInput => input.magnitude > 0.1f;
	bool cameraChanged;

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;

		CameraSelector.OnCameraChanged += (current, next) =>
		{
			cameraChanged = true;
		};

	}

	private void Start()
	{
		EnvironmentInfo.OnFallAction += StartFall;

		if (PlayerHud)
		{
			Health.OnChange += (current, max) =>
			{
				PlayerHud.SetHealth(current, max);
			};
			PlayerHud.SetHealth(Health.CurrentHealth, Health.CurrentHealth);
		}

		Launchable.OnLaunch += OnLaunch;
		Launchable.OnStopLaunch += OnStopLaunch;

		Health.OnDead += (pos) =>
		{
			GameOver.Show();
		};

		ChangeState(StartingState);

	}

	void ChangeState(IPlayerState state)
	{
		if (currentState != null)
			Debug.Log("Changing state from" + currentState.GetType().ToString() + " to " + state.GetType().Name);

		if (state is ISafe)
		{
			safe = (ISafe)state;
		}

		currentState?.Exit();
		currentState = state;
		currentState.Enter();
	}

	void StartFall()
	{
		fallTimer.ResetTimer();

		if (safe != null)
			ChangeState(safe.BackToSafe(this));
	}

	void OnLaunch()
	{
		Rigidbody.isKinematic = false;
		CharacterController.enabled = false;
	}

	void OnStopLaunch()
	{
		Rigidbody.isKinematic = true;
		CharacterController.enabled = true;
	}

	public void SetSafeGround(SafeGround safeGround)
	{
		this.safeGround = safeGround;
	}

	private void Update()
	{
		if (interactionTimer.UpdateTimer())
		{
			InputInteraction = false;
		}

		if (currentState != null && currentState.Next != null)
		{
			ChangeState(currentState.Next);
		}
	}

	private void LateUpdate()
	{
		UpdateCameraRotation();
		cameraFollow.rotation *= Quaternion.Euler(0, cameraInput.x * Time.deltaTime * cameraSpeed, 0);
	}

	void UpdateCameraRotation()
	{
		//If camera Changed, keep the rotation until the player stops pressing the input
		if (cameraChanged)
		{
			if (IsPressingInput == false)
				cameraChanged = false;
		}
		else
		{
			if (CameraSelector.CurrentCamera)
				CameraRotation = Quaternion.Euler(0, CameraSelector.CurrentCamera.transform.rotation.eulerAngles.y, 0);
		}
	}

	void OnCamera(InputValue value)
	{
		cameraInput = value.Get<Vector2>();
	}

	void OnMove(InputValue value)
	{
		input = value.Get<Vector2>();
	}

	void OnPause()
	{
		Pause.Switch();
	}

	void OnInteract()
	{
		interactionTimer.ResetTimer();
		InputInteraction = true;
	}

}
