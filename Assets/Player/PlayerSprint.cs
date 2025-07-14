using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static kawanaka.SEManager;

namespace kawanaka
{
    public class PlayerSprint : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;

        [Header("スタミナ設定")]
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float currentStamina = 100f;
        [SerializeField] private float staminaDecreaseRate = 20f;
        [SerializeField] private float staminaRecoverRate = 15f;
        [SerializeField] private float requiredLowestStamina = 10f;
        [SerializeField] private float crouchRecoverMultiplier = 2f;

        [Header("Vignette設定")]
        [SerializeField] private PostProcessVolume postProcessVolume;
        private Vignette vignette;

        private bool isRecoveringOnly = false;

        private void Start()
        {
            if (postProcessVolume != null)
            {
                if (postProcessVolume.profile.TryGetSettings(out vignette))
                {
                    vignette.intensity.overrideState = true;
                }
            }
        }

        private void Update()
        {
            bool isSprinting = Input.GetKey(KeyCode.LeftShift) && playerStatusManager.GetStatus(PlayerStatusType.IsWalk);
            bool isCrouch = playerStatusManager.GetStatus(PlayerStatusType.IsCrouch);
            bool isOption = playerStatusManager.GetStatus(PlayerStatusType.IsOption);

            if (isSprinting && !isRecoveringOnly && currentStamina > 0f)
            {
                currentStamina -= staminaDecreaseRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                playerStatusManager.SetStatus(PlayerStatusType.IsSprint, true);

                if (currentStamina <= 0f)
                {
                    isRecoveringOnly = true;
                    playerStatusManager.SetStatus(PlayerStatusType.IsSprint, false);
                }
            }
            else
            {
                if (!isSprinting && !isOption)
                {
                    float recoverRate = staminaRecoverRate;

                    if (isCrouch)
                    {
                        recoverRate *= crouchRecoverMultiplier;
                    }

                    currentStamina += recoverRate * Time.deltaTime;
                    currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

                    if (isRecoveringOnly && currentStamina >= requiredLowestStamina)
                    {
                        isRecoveringOnly = false;
                    }
                }

                playerStatusManager.SetStatus(PlayerStatusType.IsSprint, false);
            }

            UpdateVignetteIntensity();
            UpdateStaminaSE();
        }

        private int currentStaminaSEIndex = -1;
        private Coroutine staminaSECoroutine = null;

        private void UpdateStaminaSE()
        {
            int nextSE = currentStamina < maxStamina * 0.7f ? 3 : 2;

            if (nextSE != currentStaminaSEIndex)
            {
                if (staminaSECoroutine != null)
                {
                    StopCoroutine(staminaSECoroutine);
                    staminaSECoroutine = null;
                }

                staminaSECoroutine = StartCoroutine(FadeOutAndPlaySE(nextSE, SECategory.Stamina, 0.5f));
                currentStaminaSEIndex = nextSE;
            }
        }

        private IEnumerator FadeOutAndPlaySE(int seIndex, SECategory category, float fadeTime)
        {
            SEManager.Instance.StopSE(category, fadeTime);
            yield return new WaitForSeconds(fadeTime);
            SEManager.Instance.PlaySE_Looping(seIndex, category);
        }

        public void IncreaseMaxStamina(float amount)
        {
            maxStamina += amount;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        public float staminaRatio;

        private void UpdateVignetteIntensity()
        {
            if (vignette == null) return;

            vignette.intensity.overrideState = true;

            staminaRatio = Mathf.Clamp01(currentStamina / maxStamina);
            float intensityMin = 0.15f;
            float intensityMax = 0.50f;

            vignette.intensity.value = Mathf.Lerp(intensityMax, intensityMin, staminaRatio);
        }

        public float GetCurrentRatio() => staminaRatio;
        public float GetCurrentStamina() => currentStamina;
        public float GetMaxStamina() => maxStamina;
    }
}