using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class PlayerMove : MonoBehaviour
    {
        private Rigidbody playerRb;
        private PlayerStatusManager playerStatusManager;

        [Header("速度設定")]
        public float groundMoveSpeed = 5.0f;
        public float crouchSpeedMultiplier = 0.5f;
        public float sprintSpeedMultiplier = 5.0f;
        public float airMoveSpeed = 2.0f;
        public float sprintSpeedRate = 1.5f;
        [SerializeField] private float backWalkMultiplier = 0.3f;

        [Header("段差・回転・しゃがみチェック")]
        public float stepHeight = 0.5f;
        [SerializeField] float footHeight = 0.05f;
        public float rotationSpeed = 0.2f;
        [SerializeField] private Transform headCheckPoint;
        [SerializeField] private float standUpCheckDistance = 1.0f;
        [SerializeField] private LayerMask ceilingLayer;

        [Header("プレイヤー制御")]
        public Transform playerCenter;
        public GameObject crouchVisualObject;

        private void Awake()
        {
            playerRb = GetComponent<Rigidbody>();
            playerStatusManager = GetComponent<PlayerStatusManager>();
        }

        private void Update()
        {
            if (playerStatusManager.GetStatus(PlayerStatusType.IsOperation)) return;

            MovePlayer();
        }

        public void IncreaseSpeedPermanently(float amount)
        {
            groundMoveSpeed += amount;
            airMoveSpeed += amount / 2f;
        }

        private void MovePlayer()
        {
            // 入力取得
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 movementInput = new Vector3(horizontal, 0, vertical).normalized;
            Vector3 moveDirection = playerCenter.forward * movementInput.z + playerCenter.right * movementInput.x;

            // --- しゃがみ処理 ---
            bool crouchInput = Input.GetMouseButton(1);
            bool wasCrouching = playerStatusManager.GetStatus(PlayerStatusType.IsCrouch);
            bool canStand = CanStandUp();

            Debug.DrawRay(headCheckPoint.position, Vector3.up * standUpCheckDistance, canStand ? Color.green : Color.red, 0.1f);

            bool isCrouch = wasCrouching;

            if (!canStand)
            {
                isCrouch = true;
            }
            else if (crouchInput)
            {
                isCrouch = true;
                if (crouchVisualObject != null) crouchVisualObject.SetActive(false);
            }
            else if (wasCrouching && canStand)
            {
                isCrouch = false;
                if (crouchVisualObject != null) crouchVisualObject.SetActive(true);
            }

            playerStatusManager.SetStatus(PlayerStatusType.IsCrouch, isCrouch);

            // --- 移動速度決定 ---
            float currentGroundSpeed = groundMoveSpeed;
            float currentAirSpeed = airMoveSpeed;

            if (isCrouch)
            {
                currentGroundSpeed *= crouchSpeedMultiplier;
                currentAirSpeed *= crouchSpeedMultiplier;
            }

            if (movementInput.z < 0f)
            {
                currentGroundSpeed *= backWalkMultiplier;
                currentAirSpeed *= backWalkMultiplier;
            }

            bool isSprint = playerStatusManager.GetStatus(PlayerStatusType.IsSprint);
            if (!isCrouch && isSprint)
            {
                currentGroundSpeed *= sprintSpeedRate;
                currentAirSpeed *= sprintSpeedRate;
            }

            bool isGrounded = playerStatusManager.GetStatus(PlayerStatusType.IsGround);
            float moveSpeed = isGrounded ? currentGroundSpeed : currentAirSpeed;

            // --- 移動処理 ---
            Vector3 targetVelocity = moveDirection * moveSpeed;
            playerRb.velocity = new Vector3(targetVelocity.x, playerRb.velocity.y, targetVelocity.z);

            // --- 回転処理 ---
            if (moveDirection.magnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            // --- ステータス更新 ---
            playerStatusManager.SetStatus(PlayerStatusType.IsWalk, movementInput.magnitude > 0);

            // --- 段差補正 ---
            Vector3 rayStart = transform.position + Vector3.up * footHeight;
            Vector3 rayDirection = moveDirection.normalized;

            Debug.DrawRay(rayStart, rayDirection * 0.6f, Color.red);

            if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, 0.6f))
            {
                if (hit.collider.CompareTag("Ground") && hit.point.y - transform.position.y < stepHeight)
                {
                    playerRb.position = new Vector3(playerRb.position.x, hit.point.y + stepHeight, playerRb.position.z);
                }
            }
        }

        [SerializeField] private int checkRayCount = 8;         // 水平方向のレイ本数（円周に沿って）
        [SerializeField] private float checkRadius = 0.3f;      // 頭の周囲の半径

        private bool CanStandUp()
        {
            Vector3 center = headCheckPoint.position;

            for (int i = 0; i < checkRayCount; i++)
            {
                float angle = i * Mathf.PI * 2 / checkRayCount;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * checkRadius;
                Vector3 rayOrigin = center + offset;

                if (Physics.Raycast(rayOrigin, Vector3.up, standUpCheckDistance, ceilingLayer))
                {
                    Debug.DrawRay(rayOrigin, Vector3.up * standUpCheckDistance, Color.red, 0.1f);
                    return false;
                }

                Debug.DrawRay(rayOrigin, Vector3.up * standUpCheckDistance, Color.green, 0.1f);
            }

            return true;
        }
    }
}