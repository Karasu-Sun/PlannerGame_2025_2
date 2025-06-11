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
        [SerializeField] private float obstacleCheckDistance = 2f;
        [SerializeField] private float playerCheckInterval = 1f;
        [SerializeField] private float viewAngle = 120f;
        [SerializeField] private float forgetTime = 5f;

        private NavMeshAgent agent;
        private int currentPatrolIndex = 0;

        private float nextPlayerCheckTime = 0f;
        private Vector3 lastSeenPlayerPosition;
        private float timeSinceLastSeen = Mathf.Infinity;

        private enum State { Patrol, Chase, Search }
        private State currentState = State.Patrol;

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

            switch (currentState)
            {
                case State.Patrol:
                    Patrol();
                    if (IsPlayerVisible())
                    {
                        currentState = State.Chase;
                    }
                    break;
                case State.Chase:
                    ChasePlayer();
                    break;
                case State.Search:
                    SearchLastSeen();
                    break;
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

        private void ChasePlayer()
        {
            if (Time.time >= nextPlayerCheckTime)
            {
                nextPlayerCheckTime = Time.time + playerCheckInterval;
                Vector3 dirToPlayer = (player.position - transform.position).normalized;

                if (!IsObstructed(dirToPlayer) && IsPlayerVisible())
                {
                    agent.SetDestination(player.position);
                    lastSeenPlayerPosition = player.position;
                    timeSinceLastSeen = 0f;
                    return;
                }
                else
                {
                    TryAvoidObstacle();
                }
            }

            timeSinceLastSeen += Time.deltaTime;
            if (timeSinceLastSeen > forgetTime)
            {
                currentState = State.Search;
            }
        }

        private void SearchLastSeen()
        {
            agent.SetDestination(lastSeenPlayerPosition);
            if (!agent.pathPending && agent.remainingDistance < 1f)
            {
                currentState = State.Patrol;
                currentPatrolIndex = GetNearestPatrolPointIndex();
                GoToNextPatrolPoint();
            }

            if (IsPlayerVisible())
            {
                currentState = State.Chase;
            }
        }

        private void TryAvoidObstacle()
        {
            Vector3 targetDir = (player.position - transform.position).normalized;
            Vector3 sideStep = Vector3.Cross(Vector3.up, targetDir).normalized * 2f;

            foreach (Vector3 offset in new Vector3[] { sideStep, -sideStep })
            {
                Vector3 checkPos = transform.position + offset;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(checkPos, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    agent.SetDestination(checkPos);
                    return;
                }
            }
        }

        private bool IsPlayerVisible()
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToPlayer);

            if (angle < viewAngle * 0.5f && Vector3.Distance(transform.position, player.position) <= detectionRange)
            {
                if (!Physics.Linecast(transform.position, player.position, out RaycastHit hit))
                {
                    return true;
                }
                else if (hit.transform == player)
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

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * detectionRange);
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * detectionRange);
        }
    }
}