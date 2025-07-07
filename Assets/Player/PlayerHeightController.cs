using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class PlayerHeightController : MonoBehaviour
    {
        [Header("参照")]
        [SerializeField] private PlayerStatusManager playerStatusManager;
        [SerializeField] private PlayerMove playerMove;

        [Header("しゃがみ時に非表示にするオブジェクト")]
        [SerializeField] private GameObject headObject;

        [Header("天井チェック")]
        [Tooltip("プレイヤーの頭の位置")]
        [SerializeField] private Transform headCheckPoint;

        [Tooltip("立ち上がるために必要なスペース")]
        [SerializeField] private float standUpCheckDistance = 1.0f;

        [Tooltip("立ち上がりを妨げるレイヤー")]
        [SerializeField] private LayerMask ceilingLayer;

        [SerializeField] private bool previousIsCrouch = false;

        private void Update()
        {
            if (playerStatusManager == null || headObject == null) return;

            bool isCrouching = playerStatusManager.GetStatus(PlayerStatusType.IsCrouch);

            if (!isCrouching && previousIsCrouch)
            {
                if (CanStandUp())
                {
                    SetNormalHeight();
                    previousIsCrouch = false;
                }
                else
                {
                    playerStatusManager.SetStatus(PlayerStatusType.IsCrouch, true);
                }
            }
            else if (isCrouching && !previousIsCrouch)
            {
                SetCrouchHeight();
                previousIsCrouch = true;
            }
        }

        private bool CanStandUp()
        {
            return !Physics.Raycast(
                headCheckPoint.position,
                Vector3.up,
                standUpCheckDistance,
                ceilingLayer
            );
        }

        private void SetCrouchHeight()
        {
            headObject.SetActive(false);
        }

        private void SetNormalHeight()
        {
            headObject.SetActive(true);
        }

        private void OnDrawGizmosSelected()
        {
            if (headCheckPoint == null) return;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(
                headCheckPoint.position,
                headCheckPoint.position + Vector3.up * standUpCheckDistance
            );

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(
                headCheckPoint.position + Vector3.up * standUpCheckDistance,
                0.05f
            );
        }
    }
}