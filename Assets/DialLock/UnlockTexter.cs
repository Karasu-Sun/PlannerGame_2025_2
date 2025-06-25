using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace kawanaka
{
    public class UnlockTexter : MonoBehaviour
    {
        [Header("UI•\Ž¦")]
        [SerializeField] private TextMeshProUGUI unlockText;

        [SerializeField] private DialLockManager dialLockManager;

        private void Start()
        {
            if (unlockText != null)
                unlockText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!dialLockManager.isUnlocked) return;

            if (dialLockManager.isUnlocked)
            {
                ShowUnlockMessage();
            }
        }

        private void ShowUnlockMessage()
        {
            if (unlockText != null)
            {
                unlockText.gameObject.SetActive(true);
            }
        }
    }
}