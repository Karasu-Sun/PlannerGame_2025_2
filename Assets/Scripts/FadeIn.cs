using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class FadeIn : MonoBehaviour
    {

        private void Start()
        {
                StartCoroutine(DelayedFadeIn());
        }

        private IEnumerator DelayedFadeIn()
        {
            yield return null;
            StartFade();
        }

        public void StartFade()
        {
            SceneFader.Instance.StartFadeIn(() => {
                Debug.Log("フェード完了！");
            });
        }
    }
}