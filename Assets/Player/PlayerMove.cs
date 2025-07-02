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

        [Tooltip("��ވړ����x�␳")]
        [SerializeField] private float backWalkMultiplier = 0.3f;

        [Tooltip("�X�v�����g���ړ����x�␳")]
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
                // ���쒆�f��
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

            // ���Ⴊ�ݔ���
            bool isCrouch = Input.GetMouseButton(1);

            // ���Ⴊ�ݎ��͑��x������
            float currentGroundSpeed = groundMoveSpeed;
            float currentAirSpeed = airMoveSpeed;

            if (isCrouch)
            {
                currentGroundSpeed *= crouchSpeedMultiplier;
                currentAirSpeed *= crouchSpeedMultiplier;

                // ���Ⴊ�ݏ�Ԃ��X�e�[�^�X�}�l�[�W���[�ɓ`����
                playerStatusManager.SetStatus(PlayerStatusType.IsCrouch, true);
            }
            else
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsCrouch, false);
            }

            // ��ގ����x�␳
            if (movementInput.z < 0f)
            {
                currentGroundSpeed *= backWalkMultiplier;
                currentAirSpeed *= backWalkMultiplier;
            }

            // �X�v�����g��Ԃ��m�F���A�␳��K�p
            if (playerStatusManager.GetStatus(PlayerStatusType.IsSprint))
            {
                currentGroundSpeed *= sprintSpeedRate;
                currentAirSpeed *= sprintSpeedRate;
            }

            // �X�s�[�h�I��
            bool isGrounded = playerStatusManager.GetStatus(PlayerStatusType.IsGround);
            float moveSpeed = isGrounded ? currentGroundSpeed : currentAirSpeed;

            // Rigidbody�ňړ�
            Vector3 targetVelocity = moveDirection * moveSpeed;
            playerRb.velocity = new Vector3(targetVelocity.x, playerRb.velocity.y, targetVelocity.z);

            // ��]����
            if (moveDirection.magnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            // ���s�X�e�[�^�X
            playerStatusManager.SetStatus(PlayerStatusType.IsWalk, movementInput.magnitude > 0);

            // �i���Ή��iRaycast�j
            Vector3 rayStart = transform.position + Vector3.up * 0.1f;
            Vector3 rayDirection = moveDirection.normalized;

            Debug.DrawRay(rayStart, rayDirection * 0.6f, Color.red);

            // Raycast�Œi�����`�F�b�N���A�X���[�Y�ɏ��z����
            if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, 0.6f))
            {
                if (hit.collider.CompareTag("Ground") && hit.point.y - transform.position.y < stepHeight)
                {
                    // �i�������z���鏈��
                    playerRb.position = new Vector3(playerRb.position.x, hit.point.y + stepHeight, playerRb.position.z);
                }
            }
        }
    }
}