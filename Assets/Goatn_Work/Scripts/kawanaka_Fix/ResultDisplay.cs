using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ネームスペースの追加
namespace Goatn_kawanaka_Fix
{
    public class ResultDisplay : MonoBehaviour
    {
        [Header("メインテキスト設定")]
        [Tooltip("フェードイン対象のメインテキスト")]
        [SerializeField] private TextMeshProUGUI mainText;

        [Tooltip("メインテキストのフェードイン所要時間（秒）")]
        [SerializeField] private float mainFadeDuration = 1.5f;

        [Tooltip("最終的に到達するテキストの透明度")]
        [SerializeField] private float targetAlpha = 1f;

        [Tooltip("フェードイン開始時のスケール")]
        [SerializeField] private Vector3 startScale = Vector3.one * 0.8f;

        [Tooltip("フェードイン終了時のスケール")]
        [SerializeField] private Vector3 targetScale = Vector3.one;

        [Header("サブUI表示設定")]
        [Tooltip("サブテキストのフェードイン所要時間（秒）")]
        [SerializeField] private float subFadeDuration = 1f;

        [Tooltip("フェードイン対象となるサブテキスト群")]
        [SerializeField] private TextMeshProUGUI[] subTexts;

        [Header("制御対象パネル")]
        [Tooltip("フェード終了後に非表示にするパネル")]
        [SerializeField] private GameObject inputGuardPanel;

        /// <summary>
        /// 初期化処理、メインテキストのフェードインを開始
        /// </summary>
        private void Start()
        {
            InitializeUI();
            StartCoroutine(FadeInMainText());
        }

        /// <summary>
        /// UI全体の初期化
        /// </summary>
        private void InitializeUI()
        {
            if (mainText != null)
            {
                SetAlpha(mainText, 0f);
                mainText.transform.localScale = startScale;
            }

            foreach (var ui in subTexts)
            {
                if (ui != null) SetAlpha(ui, 0f);
            }
        }

        /// <summary>
        /// メインテキストを時間をかけてフェードイン表示
        /// フェード完了後、サブテキストのフェードインを開始
        /// </summary>
        private IEnumerator FadeInMainText()
        {
            float time = 0f;
            if (mainText == null) yield break;

            while (time < mainFadeDuration)
            {
                float t = time / mainFadeDuration;

                float alpha = Mathf.Lerp(0f, targetAlpha, t);
                SetAlpha(mainText, alpha);

                mainText.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

                time += Time.deltaTime;
                yield return null;
            }

            SetAlpha(mainText, targetAlpha);
            mainText.transform.localScale = targetScale;

            StartCoroutine(FadeInSubTexts());
        }

        /// <summary>
        /// サブテキスト群をフェードイン表示
        /// </summary>
        private IEnumerator FadeInSubTexts()
        {
            float time = 0f;

            while (time < subFadeDuration)
            {
                float t = time / subFadeDuration;
                float alpha = Mathf.Lerp(0f, targetAlpha, t);

                foreach (var ui in subTexts)
                {
                    if (ui != null) SetAlpha(ui, alpha);
                }

                time += Time.unscaledDeltaTime;
                yield return null;
            }

            foreach (var ui in subTexts)
            {
                if (ui != null) SetAlpha(ui, targetAlpha);
            }

            HideInputGuardPanel(inputGuardPanel);
        }

        /// <summary>
        /// 指定のTextMeshProUGUIのアルファ値のみを変更する。
        /// </summary>
        /// <param name="tmp">対象のテキスト</param>
        /// <param name="alpha">設定する透明度</param>
        private void SetAlpha(TextMeshProUGUI tmp, float alpha)
        {
            if (tmp == null) return;

            Color color = tmp.color;
            tmp.color = new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// 入力を遮断するガードパネルを非表示にします。
        /// </summary>
        public void HideInputGuardPanel(GameObject inputGuardPanel)
        {
            if (inputGuardPanel != null)
            {
                inputGuardPanel.SetActive(false);
            }
        }
    }
}