using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.VersionControl.Asset;

public class FirstPerson : MonoBehaviour, IPlayerState
{
	[SerializeField] float cameraSpeed = 10f;
	[SerializeField] Transform cameraLookAt;

	[SerializeField] float minFOV = 30;
	[SerializeField] float maxFOV = 60;
	[SerializeField] float zoomSpeed = 10;
	CameraSelector CameraSelector => FindAnyObjectByType<CameraSelector>();
	Mover Mover => GetComponent<Mover>();

	public IPlayerState Next { get; private set; }

	Vector3 cameraDirection;

	Vector3 angle;

	public void Move(Vector3 cameraDirection)
	{
		this.cameraDirection = cameraDirection.normalized;
	}

	public void OnFovDelta(InputValue value)
	{
		ChangeFov(value.Get<float>());
	}

	public void ChangeFov(float fovDelta)
	{
		if (fovDelta < 0)
			fovDelta = 1;
		else
			fovDelta = -1;

		var newFov = CameraSelector.BinocucomCamera.Lens.FieldOfView + fovDelta * zoomSpeed * Time.deltaTime;

		CameraSelector.BinocucomCamera.Lens.FieldOfView = Mathf.Clamp(newFov, minFOV, maxFOV);
	}

	public void OnBinocucom()
	{
		Next = Mover;
	}

	private void LateUpdate()
	{
		angle.y += cameraDirection.x * cameraSpeed * Time.deltaTime;

		angle.x -= cameraDirection.y * cameraSpeed * Time.deltaTime;
		angle.x = Mathf.Clamp(angle.x, -80, 80);

		cameraLookAt.eulerAngles = angle;
	}

	public void Enter()
	{
		CameraSelector.UseBinocucomCamera();
	}

	public void Exit()
	{

	}

	public IPlayerState Check()
	{
		throw new System.NotImplementedException();
	}
}
