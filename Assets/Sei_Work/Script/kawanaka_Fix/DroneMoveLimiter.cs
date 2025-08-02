using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

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

        [Header("速度補正設定(最低速度)")]
        [SerializeField] private float minSpeedMultiplier = 0.2f;

        [Header("低速警告表示（RawImage）")]
        [SerializeField] private RawImage warningRawImage;

        [Header("高度制限")]
        [SerializeField] private float warningHeightThreshold = 10.5f;
        [SerializeField] private float minHeightLimit = 10f;

        [Header("デバッグ表示")]
        [SerializeField] private bool showGizmo = true;
        [SerializeField] private Color gizmoColor = new Color(1f, 0.5f, 0f, 0.25f);

        [SerializeField] private int BreakSENum = 0;

        [SerializeField] private float resetHeight = 20f;

        private void Update()
        {
            if (!playerStatusManager.GetStatus(PlayerStatusType.IsOperation)) return;

            float distance = Vector3.Distance(transform.position, player.position);
            float t = Mathf.Clamp01(1f - (distance / maxDistance));
            float adjustedSpeed = Mathf.Lerp(minSpeedMultiplier, 1f, t);
            droneMove.SetMoveSpeedMultiplier(adjustedSpeed);

            bool shouldShowDistanceWarning = adjustedSpeed <= minSpeedMultiplier + 0.1f;
            bool shouldShowHeightWarning = transform.position.y < warningHeightThreshold && transform.position.y >= minHeightLimit;

            // 警告UI
            if (warningRawImage != null)
            {
                warningRawImage.enabled = shouldShowDistanceWarning || shouldShowHeightWarning;

                var videoPlayer = warningRawImage.GetComponent<VideoPlayer>();
                if (videoPlayer != null)
                {
                    if (warningRawImage.enabled && !videoPlayer.isPlaying)
                        videoPlayer.Play();
                    else if (!warningRawImage.enabled && videoPlayer.isPlaying)
                        videoPlayer.Pause();
                }
            }

            // 超過による操作終了
            if (distance > maxDistance)
            {
                SEManager.Instance.PlaySE_Blocking(BreakSENum, SEManager.SECategory.Drone);
                DisableDroneControl();
            }
            
            // 低高度による操作終了
            if (transform.position.y < minHeightLimit)
            {
                SEManager.Instance.PlaySE_Blocking(BreakSENum, SEManager.SECategory.Drone);
                ResetDronePosition();
                DisableDroneControl();
            }
        }
        private void ResetDronePosition()
        {
            if (player != null)
            {
                Vector3 playerPos = player.position;
                transform.position = new Vector3(playerPos.x, resetHeight, playerPos.z);
            }
        }

        private void DisableDroneControl()
        {
            playerStatusManager.SetStatus(PlayerStatusType.IsOperation, false);
            droneCamera.enabled = false;
            droneMove.SetMoveSpeedMultiplier(0f);

            // 操作終了時に警告も非表示にする
            if (warningRawImage != null)
            {
                warningRawImage.enabled = false;
                var videoPlayer = warningRawImage.GetComponent<VideoPlayer>();
                if (videoPlayer != null)
                {
                    videoPlayer.Pause();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!showGizmo || player == null) return;

            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(player.position, maxDistance);
        }
    }
}