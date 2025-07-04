using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace kawanaka
{
    public class EnemyNavAgentController : MonoBehaviour
    {
        [Header("参照")]
        [SerializeField] private EnemyAI enemyAI;
        [SerializeField] private EnemyStatusManager enemyStatusManager;

        [Header("速度設定")]
        public float normalSpeed = 2f;
        public float chaseSpeed = 5f;

        private void Update()
        {
            if (enemyStatusManager == null || enemyAI == null) return;

            bool isChasing = enemyStatusManager.GetStatus(EnemyStatusType.IsChase);
            enemyAI.baseSpeed = isChasing ? chaseSpeed : normalSpeed;
        }
    }
}