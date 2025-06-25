using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class PlayerNoiseEmitter : MonoBehaviour
    {
        [Tooltip("�ʏ�̉��̉e���͈́i���a�j")]
        [SerializeField] private float normalNoiseRadius = 5f;

        [Tooltip("���̉e���͈́i�}���j")]
        [SerializeField] private float squatNoiseRadius = 2f;

        [Tooltip("�m�C�Y���o���Ԋu�i�b�j")]
        [SerializeField] private float noiseInterval = 0.5f;

        private float nextNoiseTime = 0f;
        private PlayerStatusManager playerStatusManager;

        private float lastEmitTime = Mathf.NegativeInfinity;

        private void Awake()
        {
            playerStatusManager = GetComponent<PlayerStatusManager>();
        }

        private void Update()
        {
            if (Time.time >= nextNoiseTime && IsWalking())
            {
                EmitNoise();
                nextNoiseTime = Time.time + noiseInterval;
            }
        }

        private bool IsWalking()
        {
            return playerStatusManager != null && playerStatusManager.GetStatus(PlayerStatusType.IsWalk);
        }

        private void EmitNoise()
        {
            float currentNoiseRadius = Input.GetKey(KeyCode.LeftShift) ? squatNoiseRadius : normalNoiseRadius;

            NoiseEmitter.EmitNoise(transform.position, currentNoiseRadius);
            lastEmitTime = Time.time;
        }

        private void OnDrawGizmosSelected()
        {
            float currentNoiseRadius = Input.GetKey(KeyCode.LeftShift) ? squatNoiseRadius : normalNoiseRadius;

            Color gizmoColor = (Time.time - lastEmitTime <= 0.2f)
                ? new Color(1f, 0f, 0f, 0.4f)  // ��
                : new Color(1f, 1f, 0f, 0.3f); // ��

            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, currentNoiseRadius);
        }
    }
}