using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sei_kawanaka_Fix;
using System.Drawing;
using UnityEngine.UIElements;

// ドローン起動時に位置を修正するだけのスクリプト

namespace sei
{
    public class DroneCorrectPosition_sei : MonoBehaviour
    {
        [SerializeField] private DroneActivator droneActivator;

        [Header("有効化対象")]
        [SerializeField] private Camera droneCamera;

        [Header("Y軸位置")]
        [SerializeField] float cameraY = 30.0f;

        [Header("プレイヤーとする位置")]
        [SerializeField] private GameObject player;

        //DroneMoveLimiter_seiからの変数の受け渡し
        private DroneMoveLimiter_sei droneMoveLimiter_Sei;
        float maxDistans;
        private Vector3 dronePosition;
        //ドローン操作が初めてかどうか
        private bool farstDroneOperate = false;
        //ドローン起動時のポジションを一度だけ変えるためのフラグ
        public bool dronePositionSet = false;

        private void Start()
        {
            droneMoveLimiter_Sei = GetComponent<DroneMoveLimiter_sei>();
            maxDistans = droneMoveLimiter_Sei.maxDistance;
        }
        private void Update()
        {
            dronePosition = droneCamera.transform.position;
            float distance = Vector3.Distance(dronePosition, player.transform.position);
            
            if(!dronePositionSet && droneActivator.isOperation)
            {
                if(!farstDroneOperate)
                {
                    droneCamera.transform.position = new Vector3(player.transform.position.x, cameraY, player.transform.position.z);
                    farstDroneOperate = true;
                }
                else if(distance <= maxDistans)
                {
                    droneCamera.transform.position = dronePosition;
                }
                else if(distance > maxDistans)
                {
                    droneCamera.transform.position = new Vector3(player.transform.position.x, cameraY, player.transform.position.z);
                }
                dronePositionSet = true;
            }
            if(!droneActivator.isOperation)
            {
                dronePositionSet = false;
            }
        }
    }
}