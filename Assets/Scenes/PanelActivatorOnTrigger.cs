using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace kawanaka
{
    public class PanelActivatorOnTrigger : MonoBehaviour
    {
        [Header("対象パネルとUI")]
        [SerializeField] private GameObject targetPanel;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Slider timerSlider; // 時間ゲージ
        [SerializeField] private GameObject tutorialArea;

        [Header("表示時間・フェード設定")]
        [SerializeField] private float activeDuration = 5f;
        [SerializeField] private float fadeDuration = 0.5f;

        private Coroutine activationCoroutine;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && targetPanel != null)
            {
                if (activationCoroutine != null) StopCoroutine(activationCoroutine);
                activationCoroutine = StartCoroutine(ActivatePanelTemporarily());
            }
        }

        private IEnumerator ActivatePanelTemporarily()
        {
            targetPanel.SetActive(true);
            canvasGroup.alpha = 0f;
            timerSlider.gameObject.SetActive(true);
            timerSlider.maxValue = activeDuration;
            timerSlider.value = activeDuration;

            yield return StartCoroutine(FadeCanvasGroup(0f, 1f, fadeDuration));

            float elapsed = 0f;
            while (elapsed < activeDuration)
            {
                elapsed += Time.deltaTime;
                timerSlider.value = activeDuration - elapsed;
                yield return null;
            }

            // フェードアウト
            yield return StartCoroutine(FadeCanvasGroup(1f, 0f, fadeDuration));

            targetPanel.SetActive(false);
            timerSlider.gameObject.SetActive(false);
            tutorialArea.gameObject.SetActive(false);
        }

        private IEnumerator FadeCanvasGroup(float from, float to, float duration)
        {
            float time = 0f;
            while (time < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(from, to, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = to;
        }
    }
}
