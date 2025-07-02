using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kawanaka;

// ドローンを起動するだけのスクリプト

namespace sei_kawanaka_Fix
{
    public class DroneActivator : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;

        [Header("有効化対象")]
        [SerializeField] private GameObject droneCameraLight;
        [SerializeField] private Camera droneCamera;

        public bool isOperation;

        private void Update()
        {
            isOperation = playerStatusManager.GetStatus(PlayerStatusType.IsOperation);
            droneCameraLight.SetActive(isOperation);
            droneCamera.enabled = isOperation;
        }
    }
}