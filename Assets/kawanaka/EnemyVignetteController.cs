using kawanaka;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace kawanaka
{
    public class EnemyVignetteController : MonoBehaviour
    {
        [SerializeField] private List<EnemyStatusManager> enemyStatusManagers;

        [Header("ポストプロセス")]
        [SerializeField] private PostProcessVolume postProcessVolume;
        private Vignette vignette;

        private void Start()
        {
            if (postProcessVolume != null)
            {
                postProcessVolume.profile.TryGetSettings(out vignette);
            }
        }

        private Color targetColor;
        private float targetIntensity;

        private void Update()
        {
            bool isChase = false;
            bool isSuspicious = false;

            foreach (var enemyStatusManager in enemyStatusManagers)
            {
                if (enemyStatusManager.GetStatus(EnemyStatusType.IsChase))
                {
                    isChase = true;
                    break;
                }
                else if (enemyStatusManager.GetStatus(EnemyStatusType.IsSuspicious))
                {
                    isSuspicious = true;
                }
            }

            if (isChase)
            {
                targetColor = new Color(0.5f, 0f, 0f, 1f);
                targetIntensity = 0.45f;
            }
            else if (isSuspicious)
            {
                targetColor = new Color(0.5f, 0.45f, 0.008f, 1f);
                targetIntensity = 0.3f;
            }
            else
            {
                targetColor = Color.black;
                targetIntensity = 0f;
            }

            UpdateSeriousSE();

            if (vignette != null)
            {
                vignette.color.value = Color.Lerp(vignette.color.value, targetColor, Time.deltaTime * 5f);
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, Time.deltaTime * 5f);
            }
        }

        private int currentSeriousSEIndex = -1;

        private void UpdateSeriousSE()
        {
            int nextSE = 4;
            foreach (var enemyStatusManager in enemyStatusManagers)
            {
                if (enemyStatusManager.GetStatus(EnemyStatusType.IsChase))
                {
                    nextSE = 5;
                    break;
                }
                else if (enemyStatusManager.GetStatus(EnemyStatusType.IsSuspicious))
                {
                    nextSE = 6;
                }
            }

            if (nextSE != currentSeriousSEIndex)
            {
                SEManager.Instance.StopSE(SECategory.Serious);
                SEManager.Instance.PlaySE_Looping(nextSE, SECategory.Serious);
                currentSeriousSEIndex = nextSE;
            }
        }

        private void ApplyVignette(Color color, float intensity)
        {
            vignette.color.value = color;
            vignette.intensity.value = intensity;
        }
    }
}