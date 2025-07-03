using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using UnityEngine.UIElements;

// �h���[���N�����Ɉʒu���C�����邾���̃X�N���v�g
namespace sei_kawanaka_Fix
{
    public class DroneCorrectPosition_Master : MonoBehaviour
    {
        [SerializeField] private kawanakaver2.DroneActivator droneActivator;

        [Header("�L�����Ώ�")]
        [SerializeField] private Camera droneCamera;

        [Header("Y���ʒu")]
        [SerializeField] private float cameraY = 30.0f;

        [Header("�v���C���[�Ƃ���ʒu")]
        [SerializeField] private GameObject player;

        // �ړ������p
        private DroneMoveLimiter droneMoveLimiter_Master;
        private float maxDistance;

        // �N������p�t���O
        private bool firstDroneOperate = false;
        private bool dronePositionSet = false;

        private void Start()
        {
            droneMoveLimiter_Master = GetComponent<DroneMoveLimiter>();
            maxDistance = droneMoveLimiter_Master.maxDistance;
        }

        private void Update()
        {
            if (droneActivator == null || !droneCamera || !player) return;

            if (droneActivator.isOperation)
            {
                HandleDroneOperation();
            }
            else
            {
                ResetPositionSetFlag();
            }
        }

        private void HandleDroneOperation()
        {
            Vector3 currentPosition = droneCamera.transform.position;
            Vector3 playerPosition = player.transform.position;
            float distance = Vector3.Distance(currentPosition, playerPosition);

            if (!dronePositionSet)
            {
                if (!firstDroneOperate)
                {
                    SetDronePositionToPlayer();
                    firstDroneOperate = true;
                }
                else
                {
                    if (distance > maxDistance)
                    {
                        SetDronePositionToPlayer();
                    }
                }

                dronePositionSet = true;
            }
        }

        private void ResetPositionSetFlag()
        {
            dronePositionSet = false;
        }

        private void SetDronePositionToPlayer()
        {
            Vector3 playerPosition = player.transform.position;
            droneCamera.transform.position = new Vector3(playerPosition.x, cameraY, playerPosition.z);
        }
    }
}