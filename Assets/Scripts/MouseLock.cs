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
            // ���b�N����
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnlockCursor();
            }

            // �ă��b�N
            if (Input.GetMouseButtonDown(1))
            {
                LockCursor();
            }
        }

        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked; // �����ɌŒ�
            Cursor.visible = false; // ��\��
        }

        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None; // ���R�ړ�
            Cursor.visible = true; // �\��
        }
    }
}