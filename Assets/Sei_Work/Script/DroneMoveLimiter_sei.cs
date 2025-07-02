using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

// �h���[���̈ړ��𐧌����邾���̃X�N���v�g

namespace sei
{
    public class DroneMoveLimiter_sei : MonoBehaviour
    {
        [Header("�v���C���[�i���S�j")]
        [SerializeField] private Transform player;

        [Header("�ړ��\���� (XZ����)")]
        [SerializeField] public float maxDistance = 15f;

        [SerializeField] private PlayerStatusManager playerStatusManager;

        [Header("�Ď��Ώۂ̃J����")]
        [SerializeField] private Camera droneCamera;

        [Header("�f�o�b�O�\��")]
        [SerializeField] private bool showGizmo = true;
        [SerializeField] private Color gizmoColor = new Color(1f, 0.5f, 0f, 0.25f);

        
        private void Update()
        {
            if (!playerStatusManager.GetStatus(PlayerStatusType.IsOperation)) return;

            float distance = Vector3.Distance(transform.position, player.position);
            
            if (distance > maxDistance)
            {
                // ����𒴉�
                DisableDroneControl();
            }
        }

        private void DisableDroneControl()
        {
            playerStatusManager.SetStatus(PlayerStatusType.IsOperation, false);
            droneCamera.enabled = false;
        }

        // �f�o�b�O�p�M�Y��
        private void OnDrawGizmosSelected()
        {
            if (!showGizmo || player == null) return;

            Gizmos.color = gizmoColor;
            Vector3 centerPos = new Vector3(player.position.x, player.position.y, player.position.z);
            Gizmos.DrawWireSphere(centerPos, maxDistance);
        }
    }
}