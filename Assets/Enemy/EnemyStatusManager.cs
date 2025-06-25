using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public enum EnemyStatusType
    {
        IsIdle,
        IsWalk,
        IsChase,
        IsAttack,
        IsDead,
        IsSuspicious
    }

    public class EnemyStatusManager : MonoBehaviour
    {
        private Dictionary<EnemyStatusType, bool> statusDict = new Dictionary<EnemyStatusType, bool>();

        [SerializeField, Tooltip("現在のステータス")]
        private List<string> statusList = new List<string>();

        public event Action<EnemyStatusType, bool> OnStatusChanged;

        private void Awake()
        {
            foreach (EnemyStatusType type in Enum.GetValues(typeof(EnemyStatusType)))
            {
                statusDict[type] = false;
            }

            UpdateDebugStatus();
        }

        public bool GetStatus(EnemyStatusType statusType)
        {
            return statusDict.TryGetValue(statusType, out bool value) && value;
        }

        public void SetStatus(EnemyStatusType statusType, bool value)
        {
            if (statusDict.ContainsKey(statusType) && statusDict[statusType] != value)
            {
                statusDict[statusType] = value;
                OnStatusChanged?.Invoke(statusType, value);
                UpdateDebugStatus();
            }
        }

        private void UpdateDebugStatus()
        {
            statusList.Clear();

            foreach (var status in statusDict)
            {
                if (status.Value)
                {
                    statusList.Add(status.Key.ToString());
                }
            }
        }
    }
}