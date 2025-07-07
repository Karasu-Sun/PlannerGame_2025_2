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
        [SerializeField] private PlayerStatusManager playerStatusManager;

        private void Update()
        {
            if (playerStatusManager == null || postProcessVolume == null) return;

            bool isOption = playerStatusManager.GetStatus(PlayerStatusType.IsOption);

            if (isOption && postProcessVolume.profile != optionProfile)
            {
                postProcessVolume.profile = optionProfile;
            }
            else if (!isOption && postProcessVolume.profile != defaultProfile)
            {
                postProcessVolume.profile = defaultProfile;
            }
        }
    }
}