using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class PlayerStatusChanger : MonoBehaviour
    {
        private PlayerStatusManager playerStatusManager;

        private void Awake()
        {
            playerStatusManager = GetComponent<PlayerStatusManager>();

            // NullCheck
            if (playerStatusManager == null)
            {
                Debug.LogError("PlayerStatusManager ‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ", this);
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                bool isOperating = playerStatusManager.GetStatus(PlayerStatusType.IsOperation);

                playerStatusManager.SetStatus(PlayerStatusType.IsOperation, !isOperating);
            }
        }
    }
}