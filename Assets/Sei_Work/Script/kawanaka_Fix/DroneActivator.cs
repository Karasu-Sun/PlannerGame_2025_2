using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kawanaka;

// �h���[�����N�����邾���̃X�N���v�g

namespace sei_kawanaka_Fix
{
    public class DroneDroneActivator : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;

        [Header("�L�����Ώ�")]
        [SerializeField] private Camera droneCamera;

        public bool isOperation;

        private void Update()
        {
            isOperation = playerStatusManager.GetStatus(PlayerStatusType.IsOperation);
            droneCamera.enabled = isOperation;
        }
    }
}