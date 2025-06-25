using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka_Goatn_Fix
{
    public class GameClearManager : MonoBehaviour
    {
        public SceneChanger sceneChanger;
        public SceneFadeOut sceneFadeOut;

        [Header("�ړ���")]
        public int sceneIndexToLoad;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
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