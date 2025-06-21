using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �h���[���N�����Ɉʒu���C�����邾���̃X�N���v�g

namespace sei_kawanaka_Fix
{
    public class DroneCorrectPosition : MonoBehaviour
    {
        [SerializeField] private DroneDroneActivator droneActivator;

        [Header("�L�����Ώ�/Y���ʒu")]
        [SerializeField] private Camera droneCamera;

        [Header("�v���C���[�Ƃ���ʒu")]
        [SerializeField] private GameObject player;

        private void Update()
        {
            if (!droneActivator.isOperation)
            {
                droneCamera.transform.position = new Vector3(player.transform.position.x, 15.0f, player.transform.position.z);
            }
        }
    }
}