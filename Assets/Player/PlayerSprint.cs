using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace kawanaka
{
    public class PlayerSprint : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;

        [Header("�X�^�~�i�ݒ�")]
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float currentStamina = 100f;

        [Tooltip("�������x")]
        [SerializeField] private float staminaDecreaseRate = 20f;

        [Tooltip("�񕜑��x")]
        [SerializeField] private float staminaRecoverRate = 15f;

        [Tooltip("�K�v�Œ�X�^�~�i")]
        [SerializeField] private float requiredLowestStamina = 10f;

        [Tooltip("���Ⴊ�ݎ��񕜔{��")]
        [SerializeField] private float crouchRecoverMultiplier = 2f;

        [Header("Vignette�ݒ�")]
        [SerializeField] private PostProcessVolume postProcessVolume;
        private Vignette vignette;

        private bool isRecoveringOnly = false;

        private void Start()
        {
            if (postProcessVolume != null)
            {
                postProcessVolume.profile.TryGetSettings(out vignette);
            }
        }

        private void Update()
        {
            bool isSprinting = Input.GetKey(KeyCode.LeftShift) && playerStatusManager.GetStatus(PlayerStatusType.IsWalk);
            bool isCrouch = playerStatusManager.GetStatus(PlayerStatusType.IsCrouch);

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
                // �X�v�����g���łȂ���Ή�
                if (!isSprinting)
                {
                    float recoverRate = staminaRecoverRate;

                    // ���Ⴊ�ݎ��񕜉���
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

        // �X�^�~�i�����A�C�e���p
        public void IncreaseMaxStamina(float amount)
        {
            maxStamina += amount;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        private void UpdateVignetteIntensity()
        {
            if (vignette == null) return;

            // �X�^�~�i�c�ʂ�����ق�vignette�̋��x���グ��
            float staminaRatio = currentStamina / maxStamina;
            float intensityMin = 0.25f;
            float intensityMax = 0.65f;

            vignette.intensity.value = Mathf.Lerp(intensityMax, intensityMin, staminaRatio);
        }

        public float GetCurrentStamina() => currentStamina;
        public float GetMaxStamina() => maxStamina;
    }
}