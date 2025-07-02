using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace kawanaka
{
    public class EnemyAI : MonoBehaviour, INoiseListener
    {
        // 巡回設定
        [Header("巡回ポイント設定")]
        [Tooltip("巡回に使用するポイントのリスト")]
        [SerializeField] private List<Transform> patrolPoints;

        // プレイヤー＆視界設定
        [Header("プレイヤー＆視界設定")]
        [Tooltip("ターゲットとなるプレイヤー")]
        [SerializeField] private Transform player;

        [Tooltip("プレイヤーを感知する最大距離")]
        [SerializeField] private float detectionRange = 10f;

        [Tooltip("プレイヤーを視認できる視野角（度数）")]
        [SerializeField] private float fieldOfView = 120f;

        // AI挙動設定
        [Header("AI挙動設定")]
        [Tooltip("障害物を回避するためのチェック距離")]
        [SerializeField] private float obstacleCheckDistance = 2f;

        [Tooltip("プレイヤーの存在をチェックする間隔（秒）")]
        [SerializeField] private float playerCheckInterval = 1f;

        [Tooltip("プレイヤーを見つめる時間（秒）")]
        [SerializeField] private float lookAtDuration = 1.0f;

        [Tooltip("プレイヤーを忘れるまでの時間（秒）")]
        [SerializeField] private float forgetPlayerTime = 5f;

        [Tooltip("調査状態で立ち止まる時間（秒）")]
        [SerializeField] private float investigateWaitTime = 2f;

        [Tooltip("調査時間（秒）")]
        [SerializeField] private float searchDuration = 3f;

        [Header("移動加減速（這い動き）設定")]
        [Tooltip("通常のベース移動速度")]
        [SerializeField] public float baseSpeed = 2.5f;

        [Tooltip("速度の増減幅（±で適用）")]
        [SerializeField] private float speedAmplitude = 1.0f;

        [Tooltip("加減速の周期（秒）")]
        [SerializeField] private float speedCycleDuration = 2.0f;

        private float speedTimeElapsed = 0f;

        [Header("巡回設定")]
        [Tooltip("巡回管理")]
        [SerializeField] private Transform fixedPatrolPoint;
        private List<Transform> dynamicPatrolPoints = new List<Transform>();
        private float fixedPatrolChance = 0.2f;

        // 内部状態管理
        [HideInInspector] private float lastSeenPlayerTime = Mathf.NegativeInfinity;
        [HideInInspector] private NavMeshAgent agent;
        [HideInInspector] private int currentPatrolIndex = 0;
        [HideInInspector] private bool isChasing = false;
        [HideInInspector] private bool isLookingAtPlayer = false;
        [HideInInspector] private bool hasUnreachedPlayerPosition = false;
        [HideInInspector] private float nextPlayerCheckTime = 0f;
        [HideInInspector] private float lookAtEndTime = 0f;
        [HideInInspector] private Vector3 lastKnownPlayerPosition;

        [HideInInspector] private bool isInvestigating = false;
        [HideInInspector] private Vector3 investigatePosition;
        [HideInInspector] private float investigateStartTime = 0f;
        [HideInInspector] private bool isSearching = false;
        [HideInInspector] private float searchStartTime = 0f;

        private EnemyStatusChanger statusChanger;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            statusChanger = GetComponent<EnemyStatusChanger>();
        }

        private void Start()
        {
            if (GetCurrentPatrolPoints().Count > 0)
            {
                GoToNextPatrolPoint();
                statusChanger?.SetOnlyStatus(EnemyStatusType.IsWalk);
            }
        }

        private void Update()
        {
            speedTimeElapsed += Time.deltaTime;
            UpdateAgentSpeed();

            if (player == null || GetCurrentPatrolPoints().Count == 0) return;

            if (isLookingAtPlayer)
            {
                LookAtPlayer();
                if (Time.time >= lookAtEndTime)
                {
                    isLookingAtPlayer = false;
                    isChasing = true;
                    statusChanger?.SetOnlyStatus(EnemyStatusType.IsChase);
                }
                return;
            }

            if (isChasing)
            {
                ChasePlayer();
            }
            else if (isInvestigating)
            {
                Investigate();
            }
            else if (isSearching)
            {
                Search();
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

        public void SetDynamicPatrolPoints(List<Transform> newPoints)
        {
            // まず現在のポイントを破棄
            if (dynamicPatrolPoints != null)
            {
                foreach (var p in dynamicPatrolPoints)
                {
                    if (p != null) Destroy(p.gameObject);
                }
            }

            dynamicPatrolPoints = new List<Transform>(newPoints);

            currentPatrolIndex = 0;
            GoToNextPatrolPoint();
        }

        private void UpdateAgentSpeed()
        {
            if (agent == null) return;

            float t = (speedTimeElapsed / speedCycleDuration) * Mathf.PI * 2f;
            float speedFactor = 1.0f + Mathf.Sin(t) * (speedAmplitude / baseSpeed);
            agent.speed = baseSpeed * speedFactor;
        }

        private void StartInvestigating(Vector3 position)
        {
            isInvestigating = true;

            // 位置補正
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                investigatePosition = hit.position;
            }
            else
            {
                // 補正できなければ元の位置を使う
                investigatePosition = position;
            }

            agent.SetDestination(investigatePosition);
            investigateStartTime = 0f;
            statusChanger?.SetOnlyStatus(EnemyStatusType.IsSuspicious);
        }

        private void Investigate()
        {
            if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance + 0.2f)
                return;

            if (investigateStartTime <= 0f)
            {
                investigateStartTime = Time.time;
                agent.ResetPath();
            }

            if (IsPlayerInSight())
            {
                isInvestigating = false;
                StartLookingAtPlayer();
                return;
            }

            if (Time.time - investigateStartTime >= investigateWaitTime)
            {
                isInvestigating = false;
                StartSearching();
            }
        }

        private void StartSearching()
        {
            isSearching = true;
            searchStartTime = Time.time;
            agent.ResetPath();
            statusChanger?.SetOnlyStatus(EnemyStatusType.IsIdle);
        }

        private void Search()
        {
            if (IsPlayerInSight())
            {
                isSearching = false;
                StartLookingAtPlayer();
                return;
            }
            else if (CanSensePlayer())
            {
                isSearching = false;
                StartInvestigating(player.position);
                return;
            }

            if (Time.time - searchStartTime >= searchDuration)
            {
                isSearching = false;
                currentPatrolIndex = GetNearestPatrolPointIndex();
                GoToNextPatrolPoint();
                statusChanger?.SetOnlyStatus(EnemyStatusType.IsWalk);
            }
        }

        private void Patrol()
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPatrolPoint();
            }

            if (!isChasing && !isLookingAtPlayer)
            {
                statusChanger?.SetOnlyStatus(EnemyStatusType.IsWalk);
            }
        }
        public void OnHearNoise(Vector3 sourcePosition, float radius)
        {
            if (isChasing || isLookingAtPlayer) return;

            float distance = Vector3.Distance(transform.position, sourcePosition);
            if (distance <= radius)
            {
                StartInvestigating(sourcePosition);
            }
        }

        private void GoToNextPatrolPoint()
        {
            var currentPoints = GetCurrentPatrolPoints();
            if (currentPoints == null || currentPoints.Count == 0)
            {
                Debug.LogWarning("パトロールポイントが設定されていません");
                return;
            }

            if (currentPatrolIndex < 0 || currentPatrolIndex >= currentPoints.Count)
            {
                currentPatrolIndex = 0;
            }

            agent.SetDestination(currentPoints[currentPatrolIndex].position);

            currentPatrolIndex = (currentPatrolIndex + 1) % currentPoints.Count;
        }

        private void StartLookingAtPlayer()
        {
            agent.ResetPath();
            isLookingAtPlayer = true;
            lookAtEndTime = Time.time + lookAtDuration;

            lastKnownPlayerPosition = player.position;
            hasUnreachedPlayerPosition = true;
            statusChanger?.SetOnlyStatus(EnemyStatusType.IsIdle);
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
                    lastKnownPlayerPosition = player.position;
                    hasUnreachedPlayerPosition = true;
                    lastSeenPlayerTime = Time.time;
                    agent.SetDestination(player.position);

                    float distance = Vector3.Distance(transform.position, player.position);
                    if (distance <= agent.stoppingDistance + 0.5f)
                    {
                        statusChanger?.SetOnlyStatus(EnemyStatusType.IsAttack);
                    }
                    else
                    {
                        statusChanger?.SetOnlyStatus(EnemyStatusType.IsChase);
                    }

                    return;
                }

                if (hasUnreachedPlayerPosition)
                {
                    agent.SetDestination(lastKnownPlayerPosition);
                    float distToLastSeen = Vector3.Distance(transform.position, lastKnownPlayerPosition);

                    if (distToLastSeen <= agent.stoppingDistance + 0.5f)
                    {
                        hasUnreachedPlayerPosition = false;
                        lastSeenPlayerTime = Time.time;
                    }

                    statusChanger?.SetOnlyStatus(EnemyStatusType.IsChase);
                    return;
                }

                if (Time.time - lastSeenPlayerTime > forgetPlayerTime)
                {
                    isChasing = false;
                    currentPatrolIndex = GetNearestPatrolPointIndex();
                    GoToNextPatrolPoint();
                    statusChanger?.SetOnlyStatus(EnemyStatusType.IsWalk);
                }

                TryAvoidObstacle();
            }

            if (Vector3.Distance(transform.position, player.position) > detectionRange * 1.5f)
            {
                isChasing = false;
                hasUnreachedPlayerPosition = false;
                currentPatrolIndex = GetNearestPatrolPointIndex();
                GoToNextPatrolPoint();
                statusChanger?.SetOnlyStatus(EnemyStatusType.IsWalk);
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

        private List<Transform> GetCurrentPatrolPoints()
        {
            return (dynamicPatrolPoints != null && dynamicPatrolPoints.Count > 0)
                ? dynamicPatrolPoints
                : patrolPoints;
        }

        private int GetNearestPatrolPointIndex()
        {
            int nearest = 0;
            float minDist = float.MaxValue;

            var currentPoints = GetCurrentPatrolPoints();
            for (int i = 0; i < currentPoints.Count; i++)
            {
                float dist = Vector3.Distance(transform.position, currentPoints[i].position);
                if (dist < minDist)
                {
                    nearest = i;
                    minDist = dist;
                }
            }

            return nearest;
        }

        private bool CanSensePlayer()
        {
            float distance = Vector3.Distance(transform.position, player.position);

            bool isMakingNoise = player.TryGetComponent<PlayerStatusManager>(out var status) &&
                     status.GetStatus(PlayerStatusType.IsWalk);

            if (distance <= detectionRange * 0.6f && isMakingNoise)
            {
                if (!IsObstructed((player.position - transform.position).normalized))
                {
                    return true;
                }
            }

            return false;
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