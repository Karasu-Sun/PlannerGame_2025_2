using kawanaka;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace kawanaka
{
    public class EnemyVignetteController : MonoBehaviour
    {
        [SerializeField] private EnemyStatusManager enemyStatusManager;

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
            // 状態チェック
            if (enemyStatusManager.GetStatus(EnemyStatusType.IsChase))
            {
                targetColor = new Color(0.5f, 0f, 0f, 1f);
                targetIntensity = 0.45f;
            }
            else if (enemyStatusManager.GetStatus(EnemyStatusType.IsSuspicious))
            {
                targetColor = new Color(0.5f, 0.45f, 0.008f, 1f);
                targetIntensity = 0.3f;
            }
            else
            {
                targetColor = Color.black;
                targetIntensity = 0f;
            }

            // 補間で滑らかに変化
            vignette.color.value = Color.Lerp(vignette.color.value, targetColor, Time.deltaTime * 5f);
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, Time.deltaTime * 5f);
        }

        private void ApplyVignette(Color color, float intensity)
        {
            vignette.color.value = color;
            vignette.intensity.value = intensity;
        }
    }
}