using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class PlayerMove : MonoBehaviour
    {
        private Rigidbody playerRb;
        private PlayerStatusManager playerStatusManager;

        public float groundMoveSpeed = 5.0f;
        public float airMoveSpeed = 2.0f; // 空中での移動速度
        public float stepHeight = 0.5f; // 段差の許容範囲

        public float rotationSpeed = 0.2f;

        public Transform playerCenter; // PlayerCenterオブジェクトを参照

        private void Awake()
        {
            playerRb = GetComponent<Rigidbody>();
            playerStatusManager = GetComponent<PlayerStatusManager>();
        }

        private void Update()
        {
            if (playerStatusManager.GetStatus(PlayerStatusType.IsOperation))
            {
                // 操作中断中
                return;
            }
            MovePlayer();
        }

        public void IncreaseSpeedPermanently(float amount)
        {
            groundMoveSpeed += amount;
            airMoveSpeed += amount / 2; // 空中移動速度は少し控えめに上昇
        }

        private void MovePlayer()
        {
            // 移動入力
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // PlayerCenterの回転を考慮して移動方向を調整
            Vector3 movementInput = new Vector3(horizontal, 0, vertical).normalized;

            // PlayerCenterの回転を取得し、移動方向をワールド座標に変換
            Vector3 moveDirection = playerCenter.forward * movementInput.z + playerCenter.right * movementInput.x;

            // 進行方向にプレイヤーを回転させる
            if (moveDirection.magnitude > 0)
            {
                // 移動方向に向かって回転
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            // 移動していない場合、歩行状態をfalseに設定
            if (movementInput.magnitude > 0)
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsWalk, true);
            }
            else
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsWalk, false);
            }

            // 地面にいる時の移動
            if (playerStatusManager.GetStatus(PlayerStatusType.IsGround))
            {
                playerRb.velocity = new Vector3(moveDirection.x * groundMoveSpeed, playerRb.velocity.y, moveDirection.z * groundMoveSpeed);
            }
            else
            {
                // 空中移動
                playerRb.velocity = new Vector3(moveDirection.x * airMoveSpeed, playerRb.velocity.y, moveDirection.z * airMoveSpeed);
            }

            // レイを可視化
            Vector3 rayStart = transform.position + Vector3.up * 0.1f;
            Vector3 rayDirection = moveDirection.normalized;

            Debug.DrawRay(rayStart, rayDirection * 0.6f, Color.red);

            // Raycastで段差をチェックし、スムーズに乗り越える
            if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, 0.6f))
            {
                if (hit.collider.CompareTag("Ground") && hit.point.y - transform.position.y < stepHeight)
                {
                    // 段差を乗り越える処理
                    playerRb.position = new Vector3(playerRb.position.x, hit.point.y + stepHeight, playerRb.position.z);
                }
            }
        }
    }
}