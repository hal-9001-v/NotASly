using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    [SerializeField] float cameraSpeed = 10f;
    [SerializeField] Transform cameraLookAt;

    [SerializeField] float minFOV = 30;
    [SerializeField] float maxFOV = 60;
    [SerializeField] float zoomSpeed = 10;
    CameraSelector CameraSelector => FindObjectOfType<CameraSelector>();

    Vector3 cameraDirection;

    Vector3 angle;

    public void Attatch()
    {
        CameraSelector.UseBinocucomCamera();
    }

    public void Move(Vector3 cameraDirection)
    {
        this.cameraDirection = cameraDirection.normalized;
    }

    public void ChangeFov(float fovDelta)
    {
        if (fovDelta < 0)
            fovDelta = 1;
        else
            fovDelta = -1;
        
        var newFov = CameraSelector.BinocucomCamera.m_Lens.FieldOfView + fovDelta * zoomSpeed * Time.deltaTime;
        
        CameraSelector.BinocucomCamera.m_Lens.FieldOfView = Mathf.Clamp(newFov, minFOV, maxFOV);
    }

    private void LateUpdate()
    {
        angle.y += cameraDirection.x * cameraSpeed * Time.deltaTime;

        angle.x -= cameraDirection.y * cameraSpeed * Time.deltaTime;
        angle.x = Mathf.Clamp(angle.x, -80, 80);

        cameraLookAt.eulerAngles = angle;
    }

    public void Dettach()
    {

    }
}
