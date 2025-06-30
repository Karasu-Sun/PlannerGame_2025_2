using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class DialLockManager : MonoBehaviour
    {
        [Header("4�̃_�C����")]
        [SerializeField] private DialColumn[] dialColumns = new DialColumn[4];

        [Header("�����R�[�h�i4���j")]
        [SerializeField] private int[] correctCode = new int[4];

        [SerializeField] private bool debug_isUnlocked;
        public bool isUnlocked { get; private set; } = false;

        private void Update()
        {
            debug_isUnlocked = isUnlocked;
            if (isUnlocked) return;

            bool match = true;
            for (int i = 0; i < dialColumns.Length; i++)
            {
                if (dialColumns[i].CurrentValue != correctCode[i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                isUnlocked = true;
                Debug.Log("�J�������I");
                SEManager.Instance.PlaySE(1);
                // �C�ӂ̏����i�J����A�j���[�V�����Ȃǁj
            }
        }
    }
}