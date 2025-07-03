using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using UnityEngine.UIElements;

// ドローン起動時に位置を修正するだけのスクリプト
namespace sei_kawanaka_Fix
{
    public class DroneCorrectPosition_Master : MonoBehaviour
    {
        [SerializeField] private kawanakaver2.DroneActivator droneActivator;

        [Header("有効化対象")]
        [SerializeField] private Camera droneCamera;

        [Header("Y軸位置")]
        [SerializeField] private float cameraY = 30.0f;

        [Header("プレイヤーとする位置")]
        [SerializeField] private GameObject player;

        // 移動制限用
        private DroneMoveLimiter droneMoveLimiter_Master;
        private float maxDistance;

        // 起動制御用フラグ
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