using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class DroneBatterySystem : MonoBehaviour
    {
        [Header("�o�b�e���[�ݒ�")]
        [SerializeField] private float maxBattery = 100f;
        [SerializeField] private float batteryDrainPerSecond = 5f;
        [SerializeField] private float currentBattery = 100f;

        [Header("�h���[�����")]
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
                Debug.Log("�o�b�e���[���؂�Ă���A�h���[�����N���ł��܂���B");
            }
        }

        public void ShutdownDrone()
        {
            isActive = false;
            Debug.Log("�h���[�����o�b�e���[�؂�Œ�~���܂����B");
        }

        public void ForceDeactivate()
        {
            isActive = false;
            Debug.Log("�h���[�����蓮�Œ�~���܂����B");
        }
    }
}