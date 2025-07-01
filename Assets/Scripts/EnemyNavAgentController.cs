using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace kawanaka
{
    public class EnemyNavAgentController : MonoBehaviour
    {
        [Header("�Q��")]
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private EnemyStatusManager enemyStatusManager;

        [Header("���x�ݒ�")]
        public float normalSpeed = 2f;
        public float chaseSpeed = 5f;

        private void Update()
        {
            if (enemyStatusManager == null || agent == null) return;

            bool isChasing = enemyStatusManager.GetStatus(EnemyStatusType.IsChase);
            agent.speed = isChasing ? chaseSpeed : normalSpeed;
        }
    }
}