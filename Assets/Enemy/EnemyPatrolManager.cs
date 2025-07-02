using System.Collections;
using System.Collections.Generic;
using kawanaka;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace kawanaka
{
    public class EnemyPatrolManager : MonoBehaviour
    {
        [Header("対象の敵AI")]
        [SerializeField] private List<EnemyAI> enemies;

        [Header("生成エリア")]
        [SerializeField] private Vector3 areaCenter;
        [SerializeField] private Vector3 areaSize;

        [Header("ポイント生成設定")]
        [SerializeField] private int randomPointCount = 3;
        [SerializeField] private float minDistanceBetweenPoints = 5f;

        [Header("更新間隔（秒）")]
        [SerializeField] private float updateInterval = 30f;

        private float timer = 0f;

        private void Start()
        {
            GenerateAndAssignPatrolPoints();
            timer = 0f;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= updateInterval)
            {
                GenerateAndAssignPatrolPoints();
                timer = 0f;
            }
        }

        private void GenerateAndAssignPatrolPoints()
        {
            List<Transform> patrolPoints = GeneratePatrolPoints();

            foreach (var enemy in enemies)
            {
                enemy.SetDynamicPatrolPoints(patrolPoints);
            }
        }

        private List<Transform> GeneratePatrolPoints()
        {
            List<Transform> result = new List<Transform>();
            int attempts = 100;

            while (result.Count < randomPointCount && attempts-- > 0)
            {
                Vector3 random = areaCenter + new Vector3(
                    Random.Range(-areaSize.x / 2, areaSize.x / 2),
                    0,
                    Random.Range(-areaSize.z / 2, areaSize.z / 2)
                );

                if (NavMesh.SamplePosition(random, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    Vector3 pos = hit.position;

                    if (result.All(p => Vector3.Distance(p.position, pos) >= minDistanceBetweenPoints))
                    {
                        GameObject point = new GameObject("DynamicPatrolPoint");
                        point.transform.position = pos;
                        result.Add(point.transform);
                    }
                }
            }

            return result;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawCube(areaCenter, areaSize);
        }
    }
}