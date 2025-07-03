using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class SceneChangeListener : MonoBehaviour
    {
        public SceneChanger sceneChanger;
        public SceneFader sceneFader;

        [Header("移動先")]
        public int sceneIndexToLoad;

        public void ChangeScene()
        {
                sceneFader.StartFadeOut();
                SceneFader.Instance.StartFadeOut(() => {
                    Debug.Log("フェード完了！");
                    sceneChanger.StartChangeSceneByIndex(sceneIndexToLoad);
                });
        }
    }
}