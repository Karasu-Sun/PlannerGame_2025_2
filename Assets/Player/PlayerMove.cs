using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class PlayerMove : MonoBehaviour
    {
        private Rigidbody playerRb;
        private PlayerStatusManager playerStatusManager;

        [Header("移動速度設定")]
        public float groundMoveSpeed = 5.0f;
        public float airMoveSpeed = 2.0f;
        public float crouchSpeedMultiplier = 0.5f;
        public float sprintSpeedRate = 1.5f;
        public float backWalkMultiplier = 0.3f;

        [Header("段差判定")]
        public float stepHeight = 0.5f;

        [Header("回転速度")]
        public float rotationSpeed = 5f;

        [Header("しゃがみ判定")]
        [SerializeField] private Transform headCheckPoint;
        [SerializeField] private float standUpCheckDistance = 1.0f;
        [SerializeField] private LayerMask ceilingLayer;

        [Header("中心参照点")]
        public Transform playerCenter;

        private Vector3 inputDirection;
        private Vector3 moveDirection;
        private bool crouchInput;
        private bool sprintInput;

        [SerializeField] private GameObject headObject;

        private void Awake()
        {
            playerRb = GetComponent<Rigidbody>();
            playerStatusManager = GetComponent<PlayerStatusManager>();
        }

        private void Update()
        {
            if (playerStatusManager.GetStatus(PlayerStatusType.IsOperation)) return;

            ReadInput();
            UpdateCrouchState();
            UpdateSprintState();
        }

        private void FixedUpdate()
        {
            if (playerStatusManager.GetStatus(PlayerStatusType.IsOperation)) return;

            MovePlayer();
        }

        private void ReadInput()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            inputDirection = new Vector3(horizontal, 0, vertical).normalized;
            moveDirection = playerCenter.forward * inputDirection.z + playerCenter.right * inputDirection.x;

            crouchInput = Input.GetMouseButton(1);
            sprintInput = Input.GetKey(KeyCode.LeftShift);
        }

        private bool previousCrouchState = false;

        private void UpdateCrouchState()
        {
            bool wasCrouching = playerStatusManager.GetStatus(PlayerStatusType.IsCrouch);
            bool shouldCrouch = wasCrouching;

            if (crouchInput)
            {
                shouldCrouch = true;
            }
            else if (wasCrouching)
            {
                shouldCrouch = !CanStandUp();
            }

            if (shouldCrouch != previousCrouchState)
            {
                previousCrouchState = shouldCrouch;
                playerStatusManager.SetStatus(PlayerStatusType.IsCrouch, shouldCrouch);

                if (headObject != null)
                {
                    headObject.SetActive(!shouldCrouch);
                }
            }
        }

        private void UpdateSprintState()
        {
            playerStatusManager.SetStatus(PlayerStatusType.IsSprint, sprintInput);
        }

        private void MovePlayer()
        {
            bool isCrouching = playerStatusManager.GetStatus(PlayerStatusType.IsCrouch);
            bool isGrounded = playerStatusManager.GetStatus(PlayerStatusType.IsGround);
            bool isSprinting = playerStatusManager.GetStatus(PlayerStatusType.IsSprint);

            float currentGroundSpeed = groundMoveSpeed;
            float currentAirSpeed = airMoveSpeed;

            if (isCrouching)
            {
                currentGroundSpeed *= crouchSpeedMultiplier;
                currentAirSpeed *= crouchSpeedMultiplier;
            }

            if (inputDirection.z < 0)
            {
                currentGroundSpeed *= backWalkMultiplier;
                currentAirSpeed *= backWalkMultiplier;
            }

            if (isSprinting)
            {
                currentGroundSpeed *= sprintSpeedRate;
                currentAirSpeed *= sprintSpeedRate;
            }

            float moveSpeed = isGrounded ? currentGroundSpeed : currentAirSpeed;
            Vector3 targetVelocity = moveDirection * moveSpeed;

            playerRb.velocity = new Vector3(targetVelocity.x, playerRb.velocity.y, targetVelocity.z);

            // 回転処理
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
            }

            // 歩行ステータス更新
            playerStatusManager.SetStatus(PlayerStatusType.IsWalk, inputDirection.magnitude > 0.01f);

            // 段差対応
            if (isGrounded)
            {
                TryClimbStep();
            }
        }

        private void TryClimbStep()
        {
            Vector3 rayStart = transform.position + Vector3.up * 0.1f;
            Vector3 rayDirection = moveDirection.normalized;

            if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, 0.6f))
            {
                if (hit.collider.CompareTag("Ground") && hit.point.y - transform.position.y < stepHeight)
                {
                    Vector3 newPos = new Vector3(playerRb.position.x, hit.point.y + stepHeight, playerRb.position.z);
                    playerRb.MovePosition(newPos);
                }
            }
        }

        private bool CanStandUp()
        {
            return !Physics.Raycast(headCheckPoint.position, Vector3.up, standUpCheckDistance, ceilingLayer);
        }

        public void IncreaseSpeedPermanently(float amount)
        {
            groundMoveSpeed += amount;
            airMoveSpeed += amount / 2;
        }
    }
}