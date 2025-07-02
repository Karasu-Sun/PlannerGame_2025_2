using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;
        [SerializeField] private DialLockManager dialLockManager;

        [SerializeField] private TypewriterText typewriterText;
        [SerializeField] private int TextNumS;
        [SerializeField] private int TextNumE;

        public virtual void Interact(GameObject player)
        {
            if (dialLockManager.isUnlocked) return;

            if (playerStatusManager != null)
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsInteracting, true);
            }

            typewriterText.StartTypingByIndex(TextNumS);

            Debug.Log("インタラクト開始");
        }
        public virtual void UnInteract(GameObject player)
        {
            if (playerStatusManager != null)
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsInteracting, false);
            }

            typewriterText.StartTypingByIndex(TextNumE);
            Debug.Log("インタラクト終了");
        }
    }
}