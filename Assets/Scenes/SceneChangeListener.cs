using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class SceneChangeListener : MonoBehaviour
    {
        public SceneChanger sceneChanger;
        public SceneFadeOut sceneFadeOut;

        [Header("移動先")]
        public int sceneIndexToLoad;

        public void ChangeScene()
        {
                sceneFadeOut.StartFadeOut();
                SceneFadeOut.Instance.StartFadeOut(() => {
                    Debug.Log("フェード完了！");
                    sceneChanger.StartChangeSceneByIndex(sceneIndexToLoad);
                });
        }
    }
}