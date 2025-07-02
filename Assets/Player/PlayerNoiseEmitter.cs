using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class PlayerNoiseEmitter : MonoBehaviour
    {
        [Tooltip("’Êí‚Ì‰¹‚Ì‰e‹¿”ÍˆÍi”¼Œaj")]
        [SerializeField] private float normalNoiseRadius = 5f;

        [Tooltip("‚µ‚á‚ª‚ÝŽž‚Ì‰¹‚Ì‰e‹¿”ÍˆÍi”¼Œaj")]
        [SerializeField] private float squatNoiseRadius = 2f;

        [Tooltip("ƒXƒvƒŠƒ“ƒgŽž‚Ì‰¹‚Ì‰e‹¿”ÍˆÍi”¼Œaj")]
        [SerializeField] private float sprintNoiseRadius = 8f;

        [Tooltip("ƒmƒCƒY‚ðo‚·ŠÔŠui•bj")]
        [SerializeField] private float noiseInterval = 0.5f;

        private float nextNoiseTime = 0f;
        private float lastEmitTime = Mathf.NegativeInfinity;

        [SerializeField] private PlayerStatusManager playerStatusManager;

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
            float currentNoiseRadius = GetCurrentNoiseRadius();
            NoiseEmitter.EmitNoise(transform.position, currentNoiseRadius);
            lastEmitTime = Time.time;
        }

        private float GetCurrentNoiseRadius()
        {
            if (playerStatusManager == null)
                return normalNoiseRadius;

            if (playerStatusManager.GetStatus(PlayerStatusType.IsSprint))
                return sprintNoiseRadius;
            else if (playerStatusManager.GetStatus(PlayerStatusType.IsCrouch))
                return squatNoiseRadius;
            else
                return normalNoiseRadius;
        }

        private void OnDrawGizmosSelected()
        {
            if (playerStatusManager == null) return;

            float currentNoiseRadius = GetCurrentNoiseRadius();

            Color gizmoColor = (Time.time - lastEmitTime <= 0.2f)
                ? new Color(1f, 0f, 0f, 0.4f)
                : new Color(1f, 1f, 0f, 0.3f);

            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, currentNoiseRadius);
        }
    }
}