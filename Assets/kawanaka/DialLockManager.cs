using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class DialLockManager : MonoBehaviour
    {
        [Header("4つのダイヤル")]
        [SerializeField] private DialColumn[] dialColumns = new DialColumn[4];

        [Header("正解コード（4桁）")]
        [SerializeField] private int[] correctCode = new int[4];

        public bool isUnlocked { get; private set; } = false;

        private void Update()
        {
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
                Debug.Log("開錠成功！");
                // 任意の処理（開けるアニメーションなど）
            }
        }
    }
}