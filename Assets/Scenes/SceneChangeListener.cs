using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class SceneChangeListener : MonoBehaviour
    {
        public SceneChanger sceneChanger;
        public SceneFadeOut sceneFadeOut;

        [Header("�ړ���")]
        public int sceneIndexToLoad;

        public void ChangeScene()
        {
                sceneFadeOut.StartFadeOut();
                SceneFadeOut.Instance.StartFadeOut(() => {
                    Debug.Log("�t�F�[�h�����I");
                    sceneChanger.StartChangeSceneByIndex(sceneIndexToLoad);
                });
        }
    }
}