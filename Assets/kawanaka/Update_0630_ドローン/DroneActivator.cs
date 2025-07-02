using System;
using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

namespace kawanakaver2
{
    public class DroneActivator : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;
        [SerializeField] private DroneBatterySystem batterySystem;

        [Header("有効化対象")]
        [SerializeField] private GameObject droneCameraLight;
        [SerializeField] private Camera droneCamera;

        public bool isOperation;

        private void Update()
        {
            bool requestOperation = playerStatusManager.GetStatus(PlayerStatusType.IsOperation);

            // バッテリーが0以下なら操作不能
            if (requestOperation && batterySystem.Battery <= 0f)
            {
                batterySystem.ShutdownDrone();
                requestOperation = false;
            }

            // カメラとライトを制御
            isOperation = requestOperation;
            droneCameraLight.SetActive(isOperation);
            droneCamera.enabled = isOperation;

            // ドローンの稼働状態
            batterySystem.IsActive = isOperation;
        }
    }
}