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

        [Header("�L�����Ώ�")]
        [SerializeField] private GameObject droneCameraLight;
        [SerializeField] private Camera droneCamera;

        public bool isOperation;

        private void Update()
        {
            bool requestOperation = playerStatusManager.GetStatus(PlayerStatusType.IsOperation);

            // �o�b�e���[��0�ȉ��Ȃ瑀��s�\
            if (requestOperation && batterySystem.Battery <= 0f)
            {
                batterySystem.ShutdownDrone();
                requestOperation = false;
            }

            // �J�����ƃ��C�g�𐧌�
            isOperation = requestOperation;
            droneCameraLight.SetActive(isOperation);
            droneCamera.enabled = isOperation;

            // �h���[���̉ғ����
            batterySystem.IsActive = isOperation;
        }
    }
}