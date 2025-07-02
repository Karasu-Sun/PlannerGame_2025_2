using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// �l�[���X�y�[�X�̒ǉ�
namespace Goatn_kawanaka_Fix
{
    public class ResultDisplay : MonoBehaviour
    {
        [Header("���C���e�L�X�g�ݒ�")]
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float fadeDuration = 1.5f;
        [SerializeField] private float targetAlpha = 1f;
        [SerializeField] private Vector3 startScale = Vector3.one * 0.8f;
        [SerializeField] private Vector3 targetScale = Vector3.one;

        [Header("�T�uUI�\���ݒ�")]
        [SerializeField] private float uiFadeDuration = 1f;
        [SerializeField] private TextMeshProUGUI[] resultUI;

        // �֐������̓���
        private void Start()
        {
            if (text != null)
            {
                SetAlpha(text, 0f);
                text.transform.localScale = startScale;
            }

            foreach (var ui in resultUI)
            {
                if (ui != null) SetAlpha(ui, 0f);
            }

            StartCoroutine(FadeAndScaleIn());
        }

        private IEnumerator FadeAndScaleIn()
        {
            float time = 0f;
            Color baseColor = text.color;

            while (time < fadeDuration)
            {
                float t = time / fadeDuration;

                // �����
                float alpha = Mathf.Lerp(0f, targetAlpha, t);
                SetAlpha(text, alpha);

                // �X�P�[�����
                text.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

                time += Time.deltaTime;
                yield return null;
            }

            SetAlpha(text, targetAlpha);
            text.transform.localScale = targetScale;

            StartCoroutine(FadeInResultUI());
        }

        private IEnumerator FadeInResultUI()
        {
            float time = 0f;

            while (time < uiFadeDuration)
            {
                float t = time / uiFadeDuration;
                float alpha = Mathf.Lerp(0f, targetAlpha, t);

                foreach (var ui in resultUI)
                {
                    if (ui != null) SetAlpha(ui, alpha);
                }

                time += Time.unscaledDeltaTime;
                yield return null;
            }

            foreach (var ui in resultUI)
            {
                if (ui != null) SetAlpha(ui, targetAlpha);
            }
        }

        private void SetAlpha(TextMeshProUGUI tmp, float alpha)
        {
            if (tmp == null) return;

            Color color = tmp.color;
            tmp.color = new Color(color.r, color.g, color.b, alpha);
        }
    }
}