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
        public float crouchSpeedMultiplier = 0.5f;
        public float sprintSpeedMultiplier = 5.0f;

        public float airMoveSpeed = 2.0f;
        public float stepHeight = 0.5f;

        public float rotationSpeed = 0.2f;

        public Transform playerCenter;

        [Tooltip("後退移動速度補正")]
        [SerializeField] private float backWalkMultiplier = 0.3f;

        [Tooltip("スプリント時移動速度補正")]
        [SerializeField] private float sprintSpeedRate = 1.5f;

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
            airMoveSpeed += amount / 2;
        }

        private void MovePlayer()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 movementInput = new Vector3(horizontal, 0, vertical).normalized;
            Vector3 moveDirection = playerCenter.forward * movementInput.z + playerCenter.right * movementInput.x;

            // しゃがみ判定
            bool isCrouch = Input.GetMouseButton(1);

            // しゃがみ時は速度を減速
            float currentGroundSpeed = groundMoveSpeed;
            float currentAirSpeed = airMoveSpeed;

            if (isCrouch)
            {
                currentGroundSpeed *= crouchSpeedMultiplier;
                currentAirSpeed *= crouchSpeedMultiplier;

                // しゃがみ状態をステータスマネージャーに伝える
                playerStatusManager.SetStatus(PlayerStatusType.IsCrouch, true);
            }
            else
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsCrouch, false);
            }

            // 後退時速度補正
            if (movementInput.z < 0f)
            {
                currentGroundSpeed *= backWalkMultiplier;
                currentAirSpeed *= backWalkMultiplier;
            }

            // スプリント状態を確認し、補正を適用
            if (playerStatusManager.GetStatus(PlayerStatusType.IsSprint))
            {
                currentGroundSpeed *= sprintSpeedRate;
                currentAirSpeed *= sprintSpeedRate;
            }

            // スピード選択
            bool isGrounded = playerStatusManager.GetStatus(PlayerStatusType.IsGround);
            float moveSpeed = isGrounded ? currentGroundSpeed : currentAirSpeed;

            // Rigidbodyで移動
            Vector3 targetVelocity = moveDirection * moveSpeed;
            playerRb.velocity = new Vector3(targetVelocity.x, playerRb.velocity.y, targetVelocity.z);

            // 回転処理
            if (moveDirection.magnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            // 歩行ステータス
            playerStatusManager.SetStatus(PlayerStatusType.IsWalk, movementInput.magnitude > 0);

            // 段差対応（Raycast）
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