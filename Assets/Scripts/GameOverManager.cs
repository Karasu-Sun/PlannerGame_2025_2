using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class GameOverManager : MonoBehaviour
    {
        public SceneChanger sceneChanger;
        public SceneFader sceneFader;

        [Header("移動先")]
        public int sceneIndexToLoad;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
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