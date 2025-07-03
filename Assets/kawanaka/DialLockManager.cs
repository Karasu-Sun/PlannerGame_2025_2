using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace kawanaka
{
    public class DialLockManager : MonoBehaviour
    {
        [Header("4つのダイヤル")]
        [SerializeField] private DialColumn[] dialColumns = new DialColumn[4];

        [Header("正解コード（4桁）")]
        [SerializeField] private int[] correctCode = new int[4];

        [SerializeField] private bool debug_isUnlocked;
        public bool isUnlocked { get; private set; } = false;

        [SerializeField] private TypewriterText typewriterText;
        [SerializeField] private int TextNum;

        [SerializeField] private PlayerStatusManager playerStatusManager;

        private void Start()
        {
            GenerateRandomCode();
        }

        private void GenerateRandomCode()
        {
            for (int i = 0; i < correctCode.Length; i++)
            {
                correctCode[0] = Random.Range(0, 1);
                correctCode[1] = Random.Range(0, 9);
                correctCode[2] = Random.Range(0, 9);
                correctCode[3] = Random.Range(0, 9);
            }

            Debug.Log("生成された正解コード: " + string.Join("", correctCode));
        }

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
                playerStatusManager.SetStatus(PlayerStatusType.IsInteracting, false);
                //Debug.Log("開錠成功！");
                SEManager.Instance.PlaySE(1);
                typewriterText.StartTypingByIndex(TextNum);
            }
        }
    }
}