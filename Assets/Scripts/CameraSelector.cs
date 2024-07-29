using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraSelector : MonoBehaviour
{
	[SerializeField] CinemachineCamera followCamera;
	[SerializeField] CinemachineCamera binocucomCamera;
	[SerializeField] CinemachineCamera ventCamera;

	bool lockCamera;

	public CinemachineCamera BinocucomCamera => binocucomCamera;
	public CinemachineCamera CurrentCamera { get; private set; }

	public Action<CinemachineCamera, CinemachineCamera> OnCameraChanged;

	public void UseFollowCamera()
	{
		UseCamera(followCamera);
	}

	public void UseVentCamera()
	{
		UseCamera(ventCamera);
	}

	public void UseBinocucomCamera()
	{
		UseCamera(binocucomCamera);
	}

	public void UseCamera(CinemachineCamera camera, bool lockCamera = false)
	{
		if (this.lockCamera || CurrentCamera == camera)
			return;

		this.lockCamera = lockCamera;
		foreach (var otherCamera in FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None))
		{
			otherCamera.enabled = false;
		}

		camera.enabled = true;

		OnCameraChanged?.Invoke(CurrentCamera, camera);
		CurrentCamera = camera;
	}

	public void FreeCamera()
	{
		lockCamera = false;
	}

}
