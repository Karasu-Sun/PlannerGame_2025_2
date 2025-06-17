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
            // ���b�N����
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