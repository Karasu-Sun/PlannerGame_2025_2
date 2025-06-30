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

            Debug.Log("�C���^���N�g�J�n");
        }
        public virtual void UnInteract(GameObject player)
        {
            if (playerStatusManager != null)
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsInteracting, false);
            }

            Debug.Log("�C���^���N�g�I��");
        }
    }
}