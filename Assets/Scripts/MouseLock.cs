using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class MouseLock : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;

        private bool isValid = true;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            UnlockCursor();

            if (playerStatusManager == null)
            {
                Debug.LogWarning($"{nameof(PlayerStatusManager)} が未設定。処理をスキップ", this);
                isValid = false;
                return;
            }
        }

        private void Update()
        {
            Cursor.visible = false;

            if (!isValid) return;

            bool isOption = playerStatusManager.GetStatus(PlayerStatusType.IsOption);
            bool isInteracting = playerStatusManager.GetStatus(PlayerStatusType.IsInteracting);

            if (isOption || isInteracting)
            {
                UnlockCursor();
            }
            else
            {
                if (Cursor.lockState != CursorLockMode.Locked)
                {
                    LockCursor();
                }
            }
        }

        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;

            if (MouseImageFollower.instance != null)
            {
                MouseImageFollower.instance.SetImageVisible(false);
            }
        }

        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;

            if (MouseImageFollower.instance != null)
            {
                MouseImageFollower.instance.SetImageVisible(true);
            }
        }
    }
}