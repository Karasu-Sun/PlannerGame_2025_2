using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasCameraAssigner : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private string targetCameraTag = "MainCamera";

    private void Awake()
    {
        if (targetCanvas == null)
            targetCanvas = GetComponent<Canvas>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignCamera();
    }

    private void AssignCamera()
    {
        Camera cam = Camera.main;

        if (cam == null)
        {
            GameObject camObj = GameObject.FindGameObjectWithTag(targetCameraTag);
            if (camObj != null) cam = camObj.GetComponent<Camera>();
        }

        if (cam != null)
        {
            targetCanvas.worldCamera = cam;
        }
        else
        {
            Debug.LogWarning("ÉJÉÅÉâÇÃéÊìæÇ…é∏îs", this);
        }
    }
}