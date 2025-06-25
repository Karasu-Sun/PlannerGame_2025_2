using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class StatusAnimationManager : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private EnemyStatusManager enemyStatusManager;
        private void Awake()
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        private void OnEnable()
        {
            enemyStatusManager.OnStatusChanged += HandleStatusChange;
        }

        private void OnDisable()
        {
            enemyStatusManager.OnStatusChanged -= HandleStatusChange;
        }

        private void HandleStatusChange(EnemyStatusType status, bool isActive)
        {
            animator.SetBool(status.ToString(), isActive);
        }
    }
}