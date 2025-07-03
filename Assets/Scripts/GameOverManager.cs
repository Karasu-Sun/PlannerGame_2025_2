using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class GameOverManager : MonoBehaviour
    {
        public SceneChanger sceneChanger;
        public SceneFader sceneFader;

        [Header("�ړ���")]
        public int sceneIndexToLoad;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                sceneFader.StartFadeOut();
                SceneFader.Instance.StartFadeOut(() => {
                    Debug.Log("�t�F�[�h�����I");
                    sceneChanger.StartChangeSceneByIndex(sceneIndexToLoad);
                });
            }
        }
    }
}