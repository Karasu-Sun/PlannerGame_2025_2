using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Tooltip("シーン名一覧")]
    public List<string> sceneNames;

    public void StartChangeSceneByIndex(int index)
    {
        if (index >= 0 && index < sceneNames.Count)
        {
            ChangeScene(sceneNames[index]);
        }
        else
        {
            Debug.LogWarning("指定されたインデックスが範囲外: " + index);
        }
    }

    public void ChangeScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            StartCoroutine(ChangeSceneCoroutine(sceneName));
        }
    }

    private IEnumerator ChangeSceneCoroutine(string sceneName)
    {
        while (SceneFadeOut.Instance.isFading)
        {
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}