using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

// �h���[���̈ړ��𐧌����邾���̃X�N���v�g

namespace sei_kawanaka_Fix
{
    public class DroneMoveLimiter : MonoBehaviour
    {
        [Header("�v���C���[�i���S�j")]
        [SerializeField] private Transform player;

        [Header("�ړ��\���� (XZ����)")]
        [SerializeField] public float maxDistance = 15f;

        [SerializeField] private PlayerStatusManager playerStatusManager;

        [Header("�Ď��Ώۂ̃J����")]
        [SerializeField] private Camera droneCamera;

        [Header("���x����Ώ�")]
        [SerializeField] private CameraMove droneMove;

        [Header("�f�o�b�O�\��")]
        [SerializeField] private bool showGizmo = true;
        [SerializeField] private Color gizmoColor = new Color(1f, 0.5f, 0f, 0.25f);

        [Header("���x�␳�ݒ�(�Œᑬ�x)")]
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