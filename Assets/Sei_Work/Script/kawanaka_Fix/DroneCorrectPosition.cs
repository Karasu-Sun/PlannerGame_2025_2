using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ドローン起動時に位置を修正するだけのスクリプト

namespace sei_kawanaka_Fix
{
    public class DroneCorrectPosition : MonoBehaviour
    {
        [SerializeField] private DroneDroneActivator droneActivator;

        [Header("有効化対象")]
        [SerializeField] private Camera droneCamera;

        [Header("Y軸位置")]
        [SerializeField] float cameraY = 30.0f;

        [Header("プレイヤーとする位置")]
        [SerializeField] private GameObject player;

        private void Update()
        {
            if (!droneActivator.isOperation)
            {
                droneCamera.transform.position = new Vector3(player.transform.position.x, cameraY, player.transform.position.z);
            }
        }
    }
}