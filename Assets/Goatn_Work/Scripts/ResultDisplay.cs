using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Goatn
{
    public class ResultDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float duration = 1.5f;
        [SerializeField] private float targetAlpha = 1f;
        [SerializeField] private Vector3 startScale = Vector3.one * 0.8f;
        [SerializeField] private Vector3 targetScale = Vector3.one;

        //[SerializeField] private float delayDuration = 1f;
        [SerializeField] private float uiDuration = 1f;
        [SerializeField] private TextMeshProUGUI[] resultUI;

        void Start()
        {
            //初期状態（透明＆スケールゼロ）
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
            text.transform.localScale = startScale;

            foreach (var item in resultUI) item.color = new Color(text.color.r, text.color.g, text.color.b, 0f);

            StartCoroutine(FadeAndScaleIn());
        }

        private IEnumerator FadeAndScaleIn()
        {
            float time = 0f;
            Color startColor = text.color;

            while (time < duration)
            {
                float t = time / duration;

                // α値補間（0 → 1）
                float alpha = Mathf.Lerp(0f, targetAlpha, t);
                text.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

                // スケール補間
                text.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

                time += Time.deltaTime;
                yield return null;
            }

            // 最終値を明示的に設定（ブレ防止）
            text.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
            text.transform.localScale = targetScale;

            StartCoroutine(UIDisplay());
        }

        private IEnumerator UIDisplay()
        {
            //yield return new WaitForSeconds(delayDuration);

            float time = 0f;
            Color startColor = resultUI[0].color;

            while (time < uiDuration)
            {
                float t = time / uiDuration;

                // α値補間（0 → 1）
                float alpha = Mathf.Lerp(0f, targetAlpha, t);
                foreach (var item in resultUI) item.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

                time += Time.deltaTime;
                yield return null;
            }

            foreach (var item in resultUI) item.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        }
    }
}