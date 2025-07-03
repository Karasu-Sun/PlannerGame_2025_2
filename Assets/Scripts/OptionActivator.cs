using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

namespace kawanaka
{
    public class OptionActivator : MonoBehaviour
    {
        [SerializeField] private VolumeControl volumeControl;
        [SerializeField] private PlayerStatusManager playerStatusManager;

        private bool previousPausingState = false;
        private bool isValid = true;

        [Header("プレイヤー自動検索設定")]
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private float playerSearchInterval = 1f;
        private float playerSearchTimer = 0f;

        private void Start()
        {
            if (playerStatusManager == null)
            {
                Debug.LogWarning($"{nameof(PlayerStatusManager)} が未設定。処理をスキップ", this);
                isValid = false;
                return;
            }

            TryFindPlayer();
        }

        private void Update()
        {
            if (playerStatusManager == null)
            {
                playerSearchTimer += Time.deltaTime;
                if (playerSearchTimer >= playerSearchInterval)
                {
                    TryFindPlayer();
                    playerSearchTimer = 0f;
                }
                return;
            }

            if (!isValid) return;

            bool currentState = playerStatusManager.GetStatus(PlayerStatusType.IsOption);

            if (currentState != previousPausingState)
            {
                HandleOptionToggle(currentState);
            }

            previousPausingState = currentState;
        }

        private void TryFindPlayer()
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag(playerTag);
            if (foundPlayer == null)
            {
                PlayerStatusManager found = FindObjectOfType<PlayerStatusManager>();
                if (found != null)
                {
                    playerStatusManager = found;
                    isValid = true;
                }
            }
            else
            {
                playerStatusManager = foundPlayer.GetComponent<PlayerStatusManager>();
                if (playerStatusManager != null)
                {
                    isValid = true;
                }
            }
        }

        public void ToggleOption()
        {
            if (!isValid || playerStatusManager == null) return;

            //Debug.Log("Toggle");
            bool newState = !playerStatusManager.GetStatus(PlayerStatusType.IsOption);
            playerStatusManager.SetStatus(PlayerStatusType.IsOption, newState);
            HandleOptionToggle(newState);
            previousPausingState = newState;
        }

        private void HandleOptionToggle(bool nowPausing)
        {
            Time.timeScale = nowPausing ? 0 : 1;
            volumeControl.ToggleVolumePanel();
        }
    }
}