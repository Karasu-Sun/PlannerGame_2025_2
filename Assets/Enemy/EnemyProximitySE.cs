using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class EnemyProximitySE : MonoBehaviour
    {
        [Header("�v���C���[��Transform")]
        [SerializeField] private Transform player;

        [Header("�Đ�����SE�̐ݒ�")]
        [SerializeField] private int seIndex = 8;
        [SerializeField] private SECategory category = SECategory.Effect;

        [Header("��������")]
        [SerializeField] private float triggerDistance = 10f;

        private bool isPlayerInRange = false;

        [SerializeField] private EnemyVignetteController enemyVignetteController;

        private void Start()
        {
            if (player == null)
            {
                Debug.LogWarning("�v���C���[�����ݒ�", this);
            }
        }

        private void Update()
        {
            if (player == null) return;
            if (enemyVignetteController.isChase) return;

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= triggerDistance)
            {
                if (!isPlayerInRange)
                {
                    isPlayerInRange = true;
                    SEManager.Instance.PlaySE_Looping(seIndex, category);
                    //Debug.Log("�Đ�");
                }
            }
            else
            {
                if (isPlayerInRange)
                {
                    isPlayerInRange = false;
                    SEManager.Instance.StopSE_Index(seIndex, category);
                    //Debug.Log("�Đ���~");
                }
            }
        }
    }
}