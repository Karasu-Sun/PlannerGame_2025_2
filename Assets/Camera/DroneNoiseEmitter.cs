using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class DroneNoiseEmitter : MonoBehaviour
    {
        [Header("ノイズ半径")]
        [SerializeField] private float noiseRadius = 8f;

        [Header("ステータス参照")]
        [SerializeField] private PlayerStatusManager playerStatusManager;

        [Header("ノイズ発生対象（ドローン）")]
        [SerializeField] private Transform droneTransform;

        private bool wasOperation = false;

        private void Update()
        {
            bool isOperation = playerStatusManager.GetStatus(PlayerStatusType.IsOperation);

            if (!wasOperation && isOperation)
            {
                EmitDroneNoise();
            }

            wasOperation = isOperation;
        }

        private void EmitDroneNoise()
        {
            if (droneTransform == null)
            {
                return;
            }

            NoiseEmitter.EmitNoise(droneTransform.position, noiseRadius);
        }

        // デバッグ用ギズモ
        private void OnDrawGizmosSelected()
        {
            if (droneTransform == null) return;

            Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
            Gizmos.DrawWireSphere(droneTransform.position, noiseRadius);
        }
    }
}