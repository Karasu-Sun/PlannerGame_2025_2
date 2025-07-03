using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class GlobalAudioFader : MonoBehaviour
    {
        [Header("フェードイン時間（秒）")]
        [SerializeField] private float fadeInDuration = 1.5f;

        private void Start()
        {
            StartCoroutine(FadeInAllAudioSources());
        }

        private IEnumerator FadeInAllAudioSources()
        {
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

            List<AudioFadeInfo> fadeTargets = new List<AudioFadeInfo>();

            foreach (AudioSource source in audioSources)
            {
                if (source != null && source.enabled && source.gameObject.activeInHierarchy)
                {
                    float originalVolume = source.volume;
                    source.volume = 0f;

                    if (!source.isPlaying)
                    {
                        source.Play();
                    }

                    fadeTargets.Add(new AudioFadeInfo(source, originalVolume));
                }
            }

            float timer = 0f;
            while (timer < fadeInDuration)
            {
                float t = timer / fadeInDuration;
                foreach (var fade in fadeTargets)
                {
                    if (fade.source != null)
                    {
                        fade.source.volume = Mathf.Lerp(0f, fade.originalVolume, t);
                    }
                }

                timer += Time.deltaTime;
                yield return null;
            }

            foreach (var fade in fadeTargets)
            {
                if (fade.source != null)
                {
                    fade.source.volume = fade.originalVolume;
                }
            }
        }

        // 内部管理用クラス
        private class AudioFadeInfo
        {
            public AudioSource source;
            public float originalVolume;

            public AudioFadeInfo(AudioSource s, float v)
            {
                source = s;
                originalVolume = v;
            }
        }
    }
}