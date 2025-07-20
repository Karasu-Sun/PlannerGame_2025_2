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

        [Header("起動音")]
        [SerializeField] private int DroneActivateSE;

        public bool isOperation;
        private bool wasOperating = false;
        private bool droneActivateSEPlayed = false;

        private void Update()
        {
            bool requestOperation = playerStatusManager.GetStatus(PlayerStatusType.IsOperation);

            // バッテリーが0以下なら操作不能
            if (requestOperation && batterySystem.Battery <= 0f)
            {
                batterySystem.ShutdownDrone();
                requestOperation = false;
            }

            // 操作開始時にSEを再生
            if (requestOperation && !droneActivateSEPlayed)
            {
                SEManager.Instance.PlaySE_Blocking(DroneActivateSE, SEManager.SECategory.Drone);
                droneActivateSEPlayed = true;
            }

            // カメラとライトを制御
            isOperation = requestOperation;
            droneCameraLight.SetActive(isOperation);
            droneCamera.enabled = isOperation;

            // ドローンの稼働状態
            batterySystem.isActive = isOperation;

            // 操作終了時にSEを停止
            if (!batterySystem.isActive && wasOperating)
            {
                SEManager.Instance.StopSE(SEManager.SECategory.Drone, 0.3f);
                droneActivateSEPlayed = false;
            }

            // 状態記録
            wasOperating = isOperation;
        }
    }
}