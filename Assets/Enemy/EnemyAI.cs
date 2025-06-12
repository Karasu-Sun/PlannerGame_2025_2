using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace kawanaka
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("èÑâÒÉ|ÉCÉìÉg")]
        [SerializeField] private List<Transform> patrolPoints;

        [Header("AIê›íË")]
        [SerializeField] private Transform player;
        [SerializeField] private float detectionRange = 10f;
        [SerializeField] private float fieldOfView = 120f;
        [SerializeField] private float obstacleCheckDistance = 2f;
        [SerializeField] private float playerCheckInterval = 1f;
        [SerializeField] private float lookAtDuration = 1.0f;
        [SerializeField] private float forgetPlayerTime = 5f;

        private float lastSeenPlayerTime = Mathf.NegativeInfinity;
        private NavMeshAgent agent;
        private int currentPatrolIndex = 0;
        private bool isChasing = false;
        private bool isLookingAtPlayer = false;
        private float nextPlayerCheckTime = 0f;
        private float lookAtEndTime = 0f;
        private Vector3 lastKnownPlayerPosition;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            if (patrolPoints.Count > 0)
            {
                GoToNextPatrolPoint();
            }
        }

        private void Update()
        {
            if (player == null || patrolPoints.Count == 0) return;

            if (isLookingAtPlayer)
            {
                LookAtPlayer();
                if (Time.time >= lookAtEndTime)
                {
                    isLookingAtPlayer = false;
                    isChasing = true;
                }
                return;
            }

            if (isChasing)
            {
                ChasePlayer();
            }
            else
            {
                Patrol();

                if (IsPlayerInSight())
                {
                    StartLookingAtPlayer();
                }
            }
        }

        private void Patrol()
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPatrolPoint();
            }
        }

        private void GoToNextPatrolPoint()
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }

        private void StartLookingAtPlayer()
        {
            agent.ResetPath();
            isLookingAtPlayer = true;
            lookAtEndTime = Time.time + lookAtDuration;
        }

        private void LookAtPlayer()
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }

        private void ChasePlayer()
        {
            if (Time.time >= nextPlayerCheckTime)
            {
                nextPlayerCheckTime = Time.time + playerCheckInterval;

                Vector3 dirToPlayer = (player.position - transform.position).normalized;
                if (!IsObstructed(dirToPlayer) && IsPlayerInSight())
                {
                    agent.SetDestination(player.position);
                    lastKnownPlayerPosition = player.position;
                    lastSeenPlayerTime = Time.time;
                    return;
                }

                if (Time.time - lastSeenPlayerTime > forgetPlayerTime)
                {
                    isChasing = false;
                    currentPatrolIndex = GetNearestPatrolPointIndex();
                    GoToNextPatrolPoint();
                }

                TryAvoidObstacle();
            }

            if (Vector3.Distance(transform.position, player.position) > detectionRange * 1.5f)
            {
                isChasing = false;
                currentPatrolIndex = GetNearestPatrolPointIndex();
                GoToNextPatrolPoint();
            }
        }

        private void TryAvoidObstacle()
        {
            Vector3[] directions =
            {
            transform.right,
            -transform.right
        };

            foreach (Vector3 dir in directions)
            {
                if (!Physics.Raycast(transform.position, dir, obstacleCheckDistance))
                {
                    Vector3 candidatePos = transform.position + dir * 3f;
                    if (NavMesh.SamplePosition(candidatePos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    {
                        agent.SetDestination(hit.position);
                        return;
                    }
                }
            }
        }

        private bool IsPlayerInSight()
        {
            Vector3 dirToPlayer = player.position - transform.position;
            float angle = Vector3.Angle(transform.forward, dirToPlayer);

            if (dirToPlayer.magnitude <= detectionRange && angle <= fieldOfView * 0.5f)
            {
                if (!Physics.Raycast(transform.position, dirToPlayer.normalized, out RaycastHit hit, detectionRange))
                {
                    return false;
                }

                if (hit.transform == player)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsObstructed(Vector3 direction)
        {
            return Physics.Raycast(transform.position, direction, obstacleCheckDistance);
        }

        private int GetNearestPatrolPointIndex()
        {
            int nearest = 0;
            float minDist = float.MaxValue;

            for (int i = 0; i < patrolPoints.Count; i++)
            {
                float dist = Vector3.Distance(transform.position, patrolPoints[i].position);
                if (dist < minDist)
                {
                    nearest = i;
                    minDist = dist;
                }
            }

            return nearest;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * obstacleCheckDistance);

            Vector3 leftRay = Quaternion.Euler(0, -fieldOfView * 0.5f, 0) * transform.forward * detectionRange;
            Vector3 rightRay = Quaternion.Euler(0, fieldOfView * 0.5f, 0) * transform.forward * detectionRange;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + leftRay);
            Gizmos.DrawLine(transform.position, transform.position + rightRay);
        }
    }
}