using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

namespace kawanaka
{
    public class CameraMove : MonoBehaviour
    {
        public enum MoveMode { Lerp, SmoothDamp, Spring }

        [Header("�ړ��ݒ�")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private MoveMode moveMode = MoveMode.Lerp;
        [SerializeField] private float smoothTime = 0.3f;
        [SerializeField] private float springStiffness = 10f;
        [SerializeField] private float springDamping = 2f;

        [Header("�����l")]
        [SerializeField] private float inertiaValue = 3.0f;

        [Header("�Q��")]
        [SerializeField] private PlayerStatusManager playerStatusManager;

        private Vector3 velocity;
        private Vector3 springVelocity;
        private Vector3 inputDirection;

        private void Update()
        {
            if (!playerStatusManager.GetStatus(PlayerStatusType.IsOperation))
            {
                // ���쒆�f��
                velocity = Vector3.zero;
                springVelocity = Vector3.zero;
                inputDirection = Vector3.zero;
                return;
            }

            MoveCamera();
        }

        private float moveSpeedMultiplier = 1f;

        public void SetMoveSpeedMultiplier(float multiplier)
        {
            moveSpeedMultiplier = Mathf.Clamp01(multiplier);
        }

        private void MoveCamera()
        {
            // ���͎擾
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            float upDown = 0f;

            if (Input.GetKey(KeyCode.Space)) upDown += 1f;
            if (Input.GetKey(KeyCode.LeftShift)) upDown -= 1f;

            // ���[���h��Ԃ̌Œ莲
            Vector3 rawInput = (Vector3.right * horizontal + Vector3.forward * vertical + Vector3.up * upDown).normalized;

            if (rawInput.sqrMagnitude > 0.01f)
            {
                inputDirection = rawInput;
            }

            Vector3 targetPosition = transform.position + inputDirection * moveSpeed * moveSpeedMultiplier * Time.deltaTime;

            switch (moveMode)
            {
                case MoveMode.Lerp:
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
                    break;

                case MoveMode.SmoothDamp:
                    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                    break;

                case MoveMode.Spring:
                    Vector3 force = (targetPosition - transform.position) * springStiffness;
                    springVelocity += force * Time.deltaTime;
                    springVelocity *= Mathf.Exp(-springDamping * Time.deltaTime);
                    transform.position += springVelocity * Time.deltaTime;
                    break;
            }

            // ��������
            inputDirection = Vector3.Lerp(inputDirection, Vector3.zero, Time.deltaTime * inertiaValue);
        }
    }
}