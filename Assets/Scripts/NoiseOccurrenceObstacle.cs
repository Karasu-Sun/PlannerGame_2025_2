using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class NoiseOccurrenceObstacle : MonoBehaviour
    {
        [Header("ƒmƒCƒY”¼Œa")]
        [SerializeField] private float noiseRadius = 8f;

        [Header("ƒmƒCƒY”­¶‘ÎÛ")]
        [SerializeField] private Transform obstacleTransform;

        private int triggerEnterCount = 0;

        [Header("ƒmƒCƒYSE")]
        [SerializeField] int NoiseNum;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                triggerEnterCount++;

                EmitObstacleNoise();
                SEManager.Instance.PlaySE_Blocking(NoiseNum);

                if (triggerEnterCount >= 1)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void EmitObstacleNoise()
        {
            if (obstacleTransform == null) return;

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