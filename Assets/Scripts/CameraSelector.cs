using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSelector : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera followCamera;
    [SerializeField] CinemachineVirtualCamera binocucomCamera;
    public CinemachineVirtualCamera BinocucomCamera => binocucomCamera;
    [SerializeField] CinemachineVirtualCamera ventCamera;

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

    public void UseCamera(CinemachineVirtualCamera camera)
    {
        foreach (var otherCamera in FindObjectsOfType<CinemachineVirtualCamera>())
        {
            otherCamera.enabled = false;
        }

        camera.enabled = true;
    }

}
