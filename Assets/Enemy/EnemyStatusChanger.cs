using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class EnemyStatusChanger : MonoBehaviour
    {
        [SerializeField] private EnemyStatusManager statusManager;

        public void ChangeStatus(EnemyStatusType type, bool value)
        {
            statusManager.SetStatus(type, value);
        }

        public bool IsStatus(EnemyStatusType type)
        {
            return statusManager.GetStatus(type);
        }

        public void SetOnlyStatus(EnemyStatusType activeType)
        {
            foreach (EnemyStatusType type in Enum.GetValues(typeof(EnemyStatusType)))
            {
                statusManager.SetStatus(type, type == activeType);
            }
        }
    }
}