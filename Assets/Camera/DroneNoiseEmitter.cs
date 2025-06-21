using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class DroneNoiseEmitter : MonoBehaviour
    {
        [Header("�m�C�Y���a")]
        [SerializeField] private float noiseRadius = 8f;

        [Header("�X�e�[�^�X�Q��")]
        [SerializeField] private PlayerStatusManager playerStatusManager;

        [Header("�m�C�Y�����Ώہi�h���[���j")]
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

        // �f�o�b�O�p�M�Y��
        private void OnDrawGizmosSelected()
        {
            if (droneTransform == null) return;

            Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
            Gizmos.DrawWireSphere(droneTransform.position, noiseRadius);
        }
    }
}