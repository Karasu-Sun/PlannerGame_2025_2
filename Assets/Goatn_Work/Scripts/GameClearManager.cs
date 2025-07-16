using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka_Goatn_Fix
{
    public class GameClearManager : MonoBehaviour
    {
        public SceneChanger sceneChanger;
        public SceneFader sceneFader;

        [Header("移動先")]
        public int sceneIndexToLoad;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                sceneFader.StartFadeOut();
                SceneFader.Instance.StartFadeOut(() => {
                    Debug.Log("フェード完了！");
                    sceneChanger.StartChangeSceneByIndex(sceneIndexToLoad);
                });
            }
        }
    }
}