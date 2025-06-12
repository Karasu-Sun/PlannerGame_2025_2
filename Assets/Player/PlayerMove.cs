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
        public float airMoveSpeed = 2.0f; // �󒆂ł̈ړ����x
        public float stepHeight = 0.5f; // �i���̋��e�͈�

        public float rotationSpeed = 0.2f;

        public Transform playerCenter; // PlayerCenter�I�u�W�F�N�g���Q��

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
            airMoveSpeed += amount / 2; // �󒆈ړ����x�͏����T���߂ɏ㏸
        }

        private void MovePlayer()
        {
            // �ړ�����
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // PlayerCenter�̉�]���l�����Ĉړ������𒲐�
            Vector3 movementInput = new Vector3(horizontal, 0, vertical).normalized;

            // PlayerCenter�̉�]���擾���A�ړ����������[���h���W�ɕϊ�
            Vector3 moveDirection = playerCenter.forward * movementInput.z + playerCenter.right * movementInput.x;

            // �i�s�����Ƀv���C���[����]������
            if (moveDirection.magnitude > 0)
            {
                // �ړ������Ɍ������ĉ�]
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            // �ړ����Ă��Ȃ��ꍇ�A���s��Ԃ�false�ɐݒ�
            if (movementInput.magnitude > 0)
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsWalk, true);
            }
            else
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsWalk, false);
            }

            // �n�ʂɂ��鎞�̈ړ�
            if (playerStatusManager.GetStatus(PlayerStatusType.IsGround))
            {
                playerRb.velocity = new Vector3(moveDirection.x * groundMoveSpeed, playerRb.velocity.y, moveDirection.z * groundMoveSpeed);
            }
            else
            {
                // �󒆈ړ�
                playerRb.velocity = new Vector3(moveDirection.x * airMoveSpeed, playerRb.velocity.y, moveDirection.z * airMoveSpeed);
            }

            // ���C������
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