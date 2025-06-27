using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;

        public virtual void Interact(GameObject player)
        {
            if (playerStatusManager != null)
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsInteracting, true);
            }

            Debug.Log("インタラクト開始");
        }
    }
}