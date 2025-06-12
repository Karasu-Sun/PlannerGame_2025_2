using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class GameOverManager : MonoBehaviour
    {
        public SceneChanger sceneChanger;
        public SceneFadeOut sceneFadeOut;

        [Header("�ړ���")]
        public int sceneIndexToLoad;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                sceneFadeOut.StartFadeOut();
                SceneFadeOut.Instance.StartFadeOut(() => {
                    Debug.Log("�t�F�[�h�����I");
                    sceneChanger.StartChangeSceneByIndex(sceneIndexToLoad);
                });
            }
        }
    }
}