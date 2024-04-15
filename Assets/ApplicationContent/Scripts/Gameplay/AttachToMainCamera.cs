using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Component that attaches an object in front of the main camera with offset.</para>
/// <param name="offset">the offset</param>
/// </summary>
public sealed class AttachToMainCamera : MonoBehaviour
{
    [Range(-100, 100)]
    [SerializeField] private float _offset;

    private readonly List<Camera> mainCameras = new List<Camera>();
    private Camera currentCamera;

    private void Start()
    {
        GetMainCamerasFromScene();

        if (mainCameras.Count == 0)
        {
            CustomLogger.Error(this, "Scene does not contain MainCamera");
            return;
        }

        SwitchCamera();
    }

    private void GetMainCamerasFromScene()
    {
        mainCameras.Clear();
        Camera[] cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
        foreach (var cam in cameras)
        {
            if (cam.CompareTag("MainCamera") && cam.gameObject != gameObject)
            {
                mainCameras.Add(cam);
            }
        }
    }

    private void Update()
    {
        if (currentCamera == null)
        {
            return;
        }

        if (!currentCamera.gameObject.activeSelf)
        {
            SwitchCamera();
        }

        MapPosition();
    }

    private void SwitchCamera()
    {
        currentCamera = FindActiveCamera();
        if (currentCamera != null)
        {
            return;
        }

        GetMainCamerasFromScene();
        if (mainCameras.Count == 0)
        {
            CustomLogger.Error(this, "Scene does not contain MainCamera");
            return;
        }
        
        currentCamera = FindActiveCamera();
        if (currentCamera != null)
        {
            return;
        }
        
        CustomLogger.Error(this, "All cameras in the scene are inactive");
    }

    private Camera FindActiveCamera()
    {
        foreach (var cam in mainCameras)
        {
            if (cam.gameObject.activeSelf)
            {
                return cam;
            }
        }

        return null;
    }

    private void MapPosition()
    {
        transform.position = currentCamera.transform.TransformPoint(new Vector3(0, 0, _offset));
        transform.rotation = currentCamera.transform.rotation;
    }
}