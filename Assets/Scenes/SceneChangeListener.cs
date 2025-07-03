using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class SceneChangeListener : MonoBehaviour
    {
        public SceneChanger sceneChanger;
        public SceneFader sceneFader;

        [Header("�ړ���")]
        public int sceneIndexToLoad;

        public void ChangeScene()
        {
                sceneFader.StartFadeOut();
                SceneFader.Instance.StartFadeOut(() => {
                    Debug.Log("�t�F�[�h�����I");
                    sceneChanger.StartChangeSceneByIndex(sceneIndexToLoad);
                });
        }
    }
}