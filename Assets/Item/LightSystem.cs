using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class LightSystem : MonoBehaviour
    {
        [Header("バッテリー設定")]
        [SerializeField] private float maxCapacity = 100f;
        [SerializeField] private float batteryDrainPerSecond = 1f;
        [SerializeField] private float currentBattery = 0f;

        [Header("点灯状態")]
        [SerializeField] private bool isActive = false;

        [SerializeField] private PlayerStatusManager playerStatusManager;
        [SerializeField] private GameObject lightVisualObject;

        [SerializeField] private TypewriterText typewriterText;
        [SerializeField] private int TextNum;

        public float Battery => currentBattery;
        public bool IsActive => isActive;

        private void Update()
        {
            if (playerStatusManager == null) return;

            bool wantsToLight = playerStatusManager.GetStatus(PlayerStatusType.IsLighting);

            if (wantsToLight && currentBattery > 0f)
            {
                isActive = true;
            }
            else if (wantsToLight && currentBattery <= 0f)
            {
                // テキスト表示
                typewriterText.StartTypingByIndex(TextNum);
            }
            else if (!wantsToLight)
            {
                isActive = false;
            }

            if (isActive)
            {
                DrainBattery();

                if (currentBattery <= 0f)
                {
                    ShutdownDrone();
                }
            }

            if (lightVisualObject != null)
            {
                lightVisualObject.SetActive(isActive);
            }
        }

        private void DrainBattery()
        {
            currentBattery -= batteryDrainPerSecond * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxCapacity);
        }

        public void ChargeBattery(float amount)
        {
            currentBattery = Mathf.Clamp(currentBattery + amount, 0f, maxCapacity);
        }

        public void ShutdownDrone()
        {
            isActive = false;
            Debug.Log("バッテリー切れで停止しました。");
        }

        public void ForceDeactivate()
        {
            isActive = false;
            Debug.Log("強制的に停止しました。");
        }
    }
}