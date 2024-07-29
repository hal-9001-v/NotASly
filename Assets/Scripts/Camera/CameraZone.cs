using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
	[SerializeField] TriggerInteractable trigger;
	[SerializeField] CinemachineCamera cinemachineCamera;

	CameraSelector CameraSelector => FindFirstObjectByType<CameraSelector>();

	private void Awake()
	{
		trigger.OnEnterAction += OnEnter;
		trigger.OnExitAction += OnExit;
	}

	private void OnExit(Interactor interactor)
	{
		var player = interactor.GetComponent<Player>();

		if (player)
		{
			CameraSelector.UseFollowCamera();
			CameraSelector.FreeCamera();
		}
	}

	private void OnEnter(Interactor interactor)
	{
		var player = interactor.GetComponent<Player>();

		if (player)
			CameraSelector.UseCamera(cinemachineCamera, true);
	}

}
