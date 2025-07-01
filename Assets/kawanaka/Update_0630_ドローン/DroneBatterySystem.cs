using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class DroneBatterySystem : MonoBehaviour
    {
        [Header("バッテリー設定")]
        [SerializeField] private float maxBattery = 100f;
        [SerializeField] private float batteryDrainPerSecond = 5f;
        [SerializeField] private float currentBattery = 100f;

        [Header("ドローン状態")]
        [SerializeField] private bool isActive;

        public float Battery => currentBattery;

        public bool IsActive { get; set; } = false;

        private void Update()
        {
            if (IsActive)
            {
                DrainBattery();
                if (currentBattery <= 0f)
                {
                    ShutdownDrone();
                }
            }
        }

        private void DrainBattery()
        {
            currentBattery -= batteryDrainPerSecond * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
        }

        public void RechargeBattery(float amount)
        {
            currentBattery += amount;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
        }

        public void TryActivate()
        {
            if (currentBattery > 0f)
            {
                isActive = true;
            }
            else
            {
                Debug.Log("バッテリーが切れており、ドローンを起動できません。");
            }
        }

        public void ShutdownDrone()
        {
            isActive = false;
            Debug.Log("ドローンがバッテリー切れで停止しました。");
        }

        public void ForceDeactivate()
        {
            isActive = false;
            Debug.Log("ドローンを手動で停止しました。");
        }
    }
}