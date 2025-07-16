using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

// ドローンの移動を制限するだけのスクリプト

namespace sei
{
    public class DroneMoveLimiter_sei : MonoBehaviour
    {
        [Header("プレイヤー（中心）")]
        [SerializeField] private Transform player;

        [Header("移動可能距離 (XZ平面)")]
        [SerializeField] public float maxDistance = 15f;

        [SerializeField] private PlayerStatusManager playerStatusManager;

        [Header("監視対象のカメラ")]
        [SerializeField] private Camera droneCamera;

        [Header("デバッグ表示")]
        [SerializeField] private bool showGizmo = true;
        [SerializeField] private Color gizmoColor = new Color(1f, 0.5f, 0f, 0.25f);

        
        private void Update()
        {
            if (!playerStatusManager.GetStatus(PlayerStatusType.IsOperation)) return;

            float distance = Vector3.Distance(transform.position, player.position);
            
            if (distance > maxDistance)
            {
                // 上限を超過
                DisableDroneControl();
            }
        }

        private void DisableDroneControl()
        {
            playerStatusManager.SetStatus(PlayerStatusType.IsOperation, false);
            droneCamera.enabled = false;
        }

        // デバッグ用ギズモ
        private void OnDrawGizmosSelected()
        {
            if (!showGizmo || player == null) return;

            Gizmos.color = gizmoColor;
            Vector3 centerPos = new Vector3(player.position.x, player.position.y, player.position.z);
            Gizmos.DrawWireSphere(centerPos, maxDistance);
        }
    }
}