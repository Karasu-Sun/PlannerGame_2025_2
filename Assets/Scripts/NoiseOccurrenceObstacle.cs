using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class NoiseOccurrenceObstacle : MonoBehaviour
    {
        [Header("�m�C�Y���a")]
        [SerializeField] private float noiseRadius = 8f;

        [Header("�m�C�Y�����Ώ�")]
        [SerializeField] private Transform obstacleTransform;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EmitDroneNoise();
            }
        }

        private void EmitDroneNoise()
        {
            if (obstacleTransform == null)
            {
                return;
            }

            NoiseEmitter.EmitNoise(obstacleTransform.position, noiseRadius);
        }

        private void OnDrawGizmosSelected()
        {
            if (obstacleTransform == null) return;

            Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
            Gizmos.DrawWireSphere(obstacleTransform.position, noiseRadius);
        }
    }
}