using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class MouseLock : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;

        void Start()
        {
            LockCursor();
        }

        void Update()
        {
            // ロック解除
            if (playerStatusManager.GetStatus(PlayerStatusType.IsOption))
            {
                UnlockCursor();
            }
            else if (!playerStatusManager.GetStatus(PlayerStatusType.IsOption))
            {
                LockCursor();
            }
        }

        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked; // 中央に固定
            Cursor.visible = false; // 非表示
        }

        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None; // 自由移動
            Cursor.visible = true; // 表示
        }
    }
}