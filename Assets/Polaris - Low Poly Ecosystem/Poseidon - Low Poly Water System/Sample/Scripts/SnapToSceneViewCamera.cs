using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class SnapToSceneViewCamera : MonoBehaviour
{
    private void OnEnable()
    {
        Camera.onPreCull += OnCameraPreCull;
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }

    private void OnDisable()
    {
        Camera.onPreCull -= OnCameraPreCull;
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }

    private void OnCameraPreCull(Camera cam)
    {
        if (cam.cameraType != CameraType.SceneView)
            return;
        transform.position = cam.transform.position;
        transform.rotation = cam.transform.rotation;

    }

    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera cam)
    {
        OnCameraPreCull(cam);
    }
}
