using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kawanaka;

// �h���[�����N�����邾���̃X�N���v�g

namespace sei_kawanaka_Fix
{
    public class DroneActivator : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;

        [Header("�L�����Ώ�")]
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