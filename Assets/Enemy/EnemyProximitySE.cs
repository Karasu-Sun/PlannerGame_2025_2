using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static kawanaka.SEManager;

namespace kawanaka
{
    public class EnemyProximitySE : MonoBehaviour
    {
        [Header("プレイヤーのTransform")]
        [SerializeField] private Transform player;

        [Header("再生するSEの設定")]
        [SerializeField] private int seIndex = 8;
        [SerializeField] private SECategory category = SECategory.Effect;

        [Header("発動距離（半径）")]
        [SerializeField] private float triggerRadius = 10f;

        [Header("敵ステータスの参照")]
        [SerializeField] private EnemyStatusManager enemyStatusManager;

        private bool isPlayerInRange = false;

        private void Start()
        {
            if (player == null)
            {
                Debug.LogWarning("プレイヤーが未設定", this);
            }

            if (enemyStatusManager == null)
            {
                Debug.LogWarning("EnemyStatusManager が未設定", this);
            }
        }

        private void Update()
        {
            if (player == null || enemyStatusManager == null) return;

            if (enemyStatusManager.GetStatus(EnemyStatusType.IsChase))
            {
                StopSEIfNeeded();
                return;
            }

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= triggerRadius)
            {
                PlaySEIfNeeded();
            }
            else
            {
                StopSEIfNeeded();
            }
        }

        private void PlaySEIfNeeded()
        {
            if (!isPlayerInRange)
            {
                isPlayerInRange = true;
                SEManager.Instance.PlaySE_Looping(seIndex, category);
            }
        }

        private void StopSEIfNeeded()
        {
            if (isPlayerInRange)
            {
                isPlayerInRange = false;
                SEManager.Instance.StopSE_Index(seIndex, category);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
            Gizmos.DrawSphere(transform.position, triggerRadius);
        }
    }
}