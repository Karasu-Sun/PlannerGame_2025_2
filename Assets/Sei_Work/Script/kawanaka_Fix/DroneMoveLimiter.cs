using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

// ドローンの移動を制限するだけのスクリプト

namespace sei_kawanaka_Fix
{
    public class DroneMoveLimiter : MonoBehaviour
    {
        [Header("プレイヤー（中心）")]
        [SerializeField] private Transform player;

        [Header("移動可能距離 (XZ平面)")]
        [SerializeField] public float maxDistance = 15f;

        [SerializeField] private PlayerStatusManager playerStatusManager;

        [Header("監視対象のカメラ")]
        [SerializeField] private Camera droneCamera;

        [Header("速度制御対象")]
        [SerializeField] private CameraMove droneMove;

        [Header("デバッグ表示")]
        [SerializeField] private bool showGizmo = true;
        [SerializeField] private Color gizmoColor = new Color(1f, 0.5f, 0f, 0.25f);

        [Header("速度補正設定(最低速度)")]
        [SerializeField] private float minSpeedMultiplier = 0.2f;

        private void Update()
        {
            if (!playerStatusManager.GetStatus(PlayerStatusType.IsOperation)) return;

            float distance = Vector3.Distance(transform.position, player.position);

            float t = Mathf.Clamp01(1f - (distance / maxDistance));
            float adjustedSpeed = Mathf.Lerp(minSpeedMultiplier, 1f, t);

            droneMove.SetMoveSpeedMultiplier(adjustedSpeed);

            if (distance > maxDistance)
            {
                DisableDroneControl();
            }
        }

        private void DisableDroneControl()
        {
            playerStatusManager.SetStatus(PlayerStatusType.IsOperation, false);
            droneCamera.enabled = false;
            droneMove.SetMoveSpeedMultiplier(0f);
        }

        private void OnDrawGizmosSelected()
        {
            if (!showGizmo || player == null) return;

            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(player.position, maxDistance);
        }
    }
}