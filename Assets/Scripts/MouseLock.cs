using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class MouseLock : MonoBehaviour
    {
        void Start()
        {
            LockCursor();
        }

        void Update()
        {
            // ロック解除
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnlockCursor();
            }

            // 再ロック
            if (Input.GetMouseButtonDown(1))
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