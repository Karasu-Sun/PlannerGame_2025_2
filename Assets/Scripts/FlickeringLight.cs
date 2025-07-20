using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class FlickeringLight : MonoBehaviour
    {
        [Header("対象ライト")]
        [SerializeField] private Light targetLight;

        [Header("点滅タイミング設定")]
        [SerializeField] private float minFlickerDelay = 5.0f;
        [SerializeField] private float maxFlickerDelay = 5.0f;

        [Header("明滅時間（ランダム）")]
        [SerializeField] private float minFlickerDuration = 0.05f;
        [SerializeField] private float maxFlickerDuration = 0.3f;

        [Header("明滅回数")]
        [SerializeField] private int minFlickerCount = 1;
        [SerializeField] private int maxFlickerCount = 7;

        [Header("不安定さ調整")]
        [SerializeField] private bool enableIntensityJitter = true;
        [SerializeField] private float jitterAmount = 1.0f;

        [Header("効果音")]
        [SerializeField] private int flickerSE;
        [SerializeField] private SEManager.SECategory seCategory = SEManager.SECategory.Effect;

        [Header("プレイヤーステータス")]
        [SerializeField] private PlayerStatusManager playerStatusManager;

        private float defaultIntensity;
        private Coroutine flickerRoutine;
        private bool wasLighting = false;

        private void Start()
        {
            if (targetLight == null) targetLight = GetComponent<Light>();
            defaultIntensity = targetLight.intensity;
        }

        private void Update()
        {
            if (playerStatusManager == null) return;

            bool isLighting = playerStatusManager.GetStatus(PlayerStatusType.IsLighting);

            if (isLighting && !wasLighting)
            {
                flickerRoutine = StartCoroutine(FlickerLoop());
            }
            else if (!isLighting && wasLighting)
            {
                StopFlickering();
            }

            wasLighting = isLighting;
        }

        private IEnumerator FlickerLoop()
        {
            while (true)
            {
                float waitTime = Random.Range(minFlickerDelay, maxFlickerDelay);
                yield return new WaitForSeconds(waitTime);

                int flickerCount = Random.Range(minFlickerCount, maxFlickerCount + 1);
                for (int i = 0; i < flickerCount; i++)
                {
                    float duration = Random.Range(minFlickerDuration, maxFlickerDuration);

                    targetLight.enabled = false;
                    PlayFlickerSE();
                    yield return new WaitForSeconds(duration * 0.5f);

                    targetLight.enabled = true;
                    PlayFlickerSE();
                    yield return new WaitForSeconds(duration * 0.5f);
                }

                if (enableIntensityJitter)
                {
                    float jitter = Random.Range(-jitterAmount, jitterAmount);
                    targetLight.intensity = Mathf.Max(0, defaultIntensity + jitter);
                }
            }
        }

        private void PlayFlickerSE()
        {
            SEManager.Instance.PlaySE(flickerSE, seCategory);
        }

        public void StopFlickering()
        {
            if (flickerRoutine != null)
            {
                StopCoroutine(flickerRoutine);
                flickerRoutine = null;
            }
            targetLight.enabled = true;
            targetLight.intensity = defaultIntensity;
        }
    }
}