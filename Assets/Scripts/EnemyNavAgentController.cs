using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace kawanaka
{
    public class EnemyNavAgentController : MonoBehaviour
    {
        [Header("éQè∆")]
        [SerializeField] private EnemyAI enemyAI;
        [SerializeField] private EnemyStatusManager enemyStatusManager;

        [Header("ë¨ìxê›íË")]
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