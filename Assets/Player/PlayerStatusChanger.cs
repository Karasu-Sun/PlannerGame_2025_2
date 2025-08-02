using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static kawanaka.SEManager;

namespace kawanaka
{
    public class PlayerStatusChanger : MonoBehaviour
    {
        private PlayerStatusManager playerStatusManager;
        [SerializeField]
        private DroneBatterySystem DroneBatterySystem;
        private void Awake()
        {
            playerStatusManager = GetComponent<PlayerStatusManager>();

            // NullCheck
            if (playerStatusManager == null)
            {
                Debug.LogError("PlayerStatusManager Ç™å©Ç¬Ç©ÇËÇ‹ÇπÇÒ", this);
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (playerStatusManager == null) return;

            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Climbable"))
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsGround, true);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (playerStatusManager == null) return;

            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Climbable"))
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsGround, false);
            }
        }

        [Header("éQè∆")]
        [SerializeField] private QKeyActivationTrigger qKeyActivationTrigger;

        private void Update()
        {
            float DroneBattery = DroneBatterySystem.Battery;

            if (Input.GetKeyDown(KeyCode.Q) && qKeyActivationTrigger.isActivated)
            {
                if (playerStatusManager.GetStatus(PlayerStatusType.IsOption)) return;
                if (DroneBattery <= 0) return;

                bool isOperating = playerStatusManager.GetStatus(PlayerStatusType.IsOperation);

                playerStatusManager.SetStatus(PlayerStatusType.IsOperation, !isOperating);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (playerStatusManager.GetStatus(PlayerStatusType.IsOperation)) return;

                bool isPausing = playerStatusManager.GetStatus(PlayerStatusType.IsOption);

                playerStatusManager.SetStatus(PlayerStatusType.IsOption, !isPausing);
            }

            bool lightInput = Input.GetMouseButton(0);

            if (lightInput)
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsLighting, true);
            }
            else
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsLighting, false);

            }
        }
    }
}