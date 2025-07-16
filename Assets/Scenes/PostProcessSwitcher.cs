using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace kawanaka
{
    public class PostProcessSwitcher : MonoBehaviour
    {
        [Header("対象のPostProcessVolume")]
        [SerializeField] private PostProcessVolume postProcessVolume;

        [Header("切り替え用Profile")]
        [SerializeField] private PostProcessProfile defaultProfile;
        [SerializeField] private PostProcessProfile optionProfile;

        [Header("ステータス参照")]
        [SerializeField] private PlayerSprint playerSprint;
        [SerializeField] private PlayerStatusManager playerStatusManager;

        private PostProcessProfile currentProfile;
        private Vignette vignette;

        private void Update()
        {
            if (playerSprint == null || postProcessVolume == null) return;

            bool isOption = playerStatusManager.GetStatus(PlayerStatusType.IsOption);
            PostProcessProfile targetProfile = isOption ? optionProfile : defaultProfile;

            // Profileの切り替えがあった場合のみ
            if (postProcessVolume.profile != targetProfile)
            {
                postProcessVolume.profile = targetProfile;
                currentProfile = targetProfile;

                // 新しい profile から vignette を再取得
                if (currentProfile.TryGetSettings(out vignette))
                {
                    vignette.intensity.overrideState = true; // 忘れずに
                    Debug.Log("[PostProcessSwitcher] Vignette 取得完了");
                }
                else
                {
                    vignette = null;
                    Debug.LogWarning("[PostProcessSwitcher] Vignette が Profile に含まれていません");
                }
            }

            // ビネット強度を動的に変化させる場合はここで vignette にアクセス可能
            //if (!isOption && vignette != null)
            //{
            //    float staminaRatio = Mathf.Clamp01(playerSprint.GetCurrentRatio());
            //    float intensityMin = 0.1f;
            //    float intensityMax = 0.1f;
            //    vignette.intensity.value = Mathf.Lerp(intensityMax, intensityMin, staminaRatio);
            //}
        }
    }
}