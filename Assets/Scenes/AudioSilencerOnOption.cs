using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class AudioSilencerOnOption : MonoBehaviour
    {
        [Header("自動検索対象（プレイヤー）")]
        [SerializeField] private PlayerStatusManager playerStatusManager;

        [Header("無音化するAudioSource一覧")]
        [SerializeField] private List<AudioSource> targetAudioSources;

        private Dictionary<AudioSource, float> originalVolumes = new Dictionary<AudioSource, float>();
        [SerializeField] private bool isSilenced = false;

        [Header("プレイヤー自動検索設定")]
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private float playerSearchInterval = 1f;
        private float playerSearchTimer = 0f;

        private void Start()
        {
            foreach (var source in targetAudioSources)
            {
                if (source != null)
                {
                    originalVolumes[source] = source.volume;
                }
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

            bool isOption = playerStatusManager.GetStatus(PlayerStatusType.IsOption);

            if (isOption && !isSilenced)
            {
                MuteAll();
            }
            else if (!isOption && isSilenced)
            {
                RestoreAll();
            }
        }

        private void TryFindPlayer()
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag(playerTag);

            if (foundPlayer == null)
            {
                PlayerStatusManager found = FindObjectOfType<PlayerStatusManager>();
                if (found != null) playerStatusManager = found;
            }
            else
            {
                playerStatusManager = foundPlayer.GetComponent<PlayerStatusManager>();
            }
        }

        private void MuteAll()
        {
            foreach (var source in targetAudioSources)
            {
                if (source != null) source.volume = 0f;
            }
            isSilenced = true;
        }

        private void RestoreAll()
        {
            foreach (var source in targetAudioSources)
            {
                if (source != null) source.volume = 1f;
            }
            isSilenced = false;
        }
    }
}