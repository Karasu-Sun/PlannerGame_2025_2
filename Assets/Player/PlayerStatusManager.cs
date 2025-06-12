using System;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public enum PlayerStatusType
    {
        IsGround,
        IsStand,
        IsJump,
        IsWalk,
        IsAttack,
        IsCottonHold,
        IsOperation
    }

    public class PlayerStatusManager : MonoBehaviour
    {
        private Dictionary<PlayerStatusType, bool> statusDict = new Dictionary<PlayerStatusType, bool>();

        [SerializeField, Tooltip("現在のステータス")]
        private List<string> statusList = new List<string>();

        public event Action<PlayerStatusType, bool> OnStatusChanged;

        [Header("Health Settings")]
        [SerializeField] private int maxHealth = 3;
        private int currentHealth;

        public int MaxHealth => maxHealth;
        public event Action<int> OnHealthChanged;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        // ステータスの初期化
        private void Awake()
        {
            foreach (PlayerStatusType type in Enum.GetValues(typeof(PlayerStatusType)))
            {
                statusDict[type] = false;
            }

            UpdateDebugStatus();
        }

        public void ModifyHealth(int amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }

        // ステータスの取得
        public bool GetStatus(PlayerStatusType statusType)
        {
            return statusDict.TryGetValue(statusType, out bool value) && value;
        }

        // ステータスの変更
        public void SetStatus(PlayerStatusType statusType, bool value)
        {
            if (statusDict.ContainsKey(statusType) && statusDict[statusType] != value)
            {
                statusDict[statusType] = value;
                OnStatusChanged?.Invoke(statusType, value);

                UpdateDebugStatus();
            }
        }

        // 表示ステータスの更新
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