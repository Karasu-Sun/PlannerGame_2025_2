using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public enum SECategory
    {
        Main,
        Stamina,
        Serious,
        Effect,
        System,
        Environment
    }

    [System.Serializable]
    public class AudioCategorySource
    {
        public SECategory category;
        public AudioSource source;
    }

    public class SEManager : MonoBehaviour
    {
        public static SEManager Instance { get; private set; }

        [SerializeField] private AudioClip[] soundEffects;

        [Header("カテゴリごとのAudioSource設定")]
        [SerializeField] private List<AudioCategorySource> categorySources = new List<AudioCategorySource>();

        private Dictionary<SECategory, AudioSource> sourceMap = new Dictionary<SECategory, AudioSource>();

        [Header("PlaySE_Blocking 状態保持")]
        [SerializeField] private bool isPlayingSE = false;
        public bool IsPlayingSE => isPlayingSE;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeSourceMap();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeSourceMap()
        {
            foreach (var entry in categorySources)
            {
                if (entry.source != null && !sourceMap.ContainsKey(entry.category))
                {
                    sourceMap[entry.category] = entry.source;
                }
            }

            if (!sourceMap.ContainsKey(SECategory.Main))
            {
                Debug.LogWarning("SEManager: SECategory.Main の AudioSource が設定されていません。");
            }
        }

        public AudioClip GetClip(int index)
        {
            if (index >= 0 && index < soundEffects.Length)
            {
                return soundEffects[index];
            }

            Debug.LogWarning($"SE Clipが見つかりません: Index {index}");
            return null;
        }

        public void PlaySE(int index, SECategory category = SECategory.Main)
        {
            if (!sourceMap.TryGetValue(category, out var source)) return;

            AudioClip clip = GetClip(index);
            if (clip != null)
            {
                source.PlayOneShot(clip);
            }
        }

        public void PlaySE(string name, SECategory category = SECategory.Main)
        {
            if (!sourceMap.TryGetValue(category, out var source)) return;

            AudioClip clip = Array.Find(soundEffects, se => se.name == name);
            if (clip != null)
            {
                source.PlayOneShot(clip);
            }
            else
            {
                Debug.LogWarning($"SEが見つかりません: {name}");
            }
        }

        public void PlaySE_Blocking(string name)
        {
            if (isPlayingSE) return;

            AudioClip clip = Array.Find(soundEffects, se => se.name == name);
            if (clip != null && sourceMap.TryGetValue(SECategory.Main, out var source))
            {
                StartCoroutine(PlayAndWait(source, clip));
            }
            else
            {
                Debug.LogWarning($"SEが見つかりません: {name}");
            }
        }

        public void PlaySE_Blocking(int index)
        {
            if (isPlayingSE) return;

            AudioClip clip = GetClip(index);
            if (clip != null && sourceMap.TryGetValue(SECategory.Main, out var source))
            {
                StartCoroutine(PlayAndWait(source, clip));
            }
        }

        private HashSet<SECategory> loopingCategories = new HashSet<SECategory>();

        public void PlaySE_Looping(int index, SECategory category = SECategory.Main)
        {
            if (!sourceMap.TryGetValue(category, out var source)) return;

            AudioClip clip = GetClip(index);
            if (clip == null) return;

            if (source.clip != clip)
            {
                source.clip = clip;
                source.loop = true;
                source.Play();

                loopingCategories.Add(category); // ループ中の記録
            }
        }

        private Dictionary<SECategory, Coroutine> fadeOutCoroutines = new Dictionary<SECategory, Coroutine>();

        public void StopSE(SECategory category = SECategory.Main, float fadeTime = 0.5f)
        {
            if (!sourceMap.TryGetValue(category, out var source)) return;

            if (source.isPlaying)
            {
                if (fadeOutCoroutines.ContainsKey(category))
                {
                    StopCoroutine(fadeOutCoroutines[category]);
                }

                fadeOutCoroutines[category] = StartCoroutine(FadeOutAndStop(source, category, fadeTime));
            }

            if (category == SECategory.Main)
                isPlayingSE = false;
        }

        public void StopSE_Index(int index, SECategory category = SECategory.Main, float fadeTime = 0.5f)
        {
            if (!sourceMap.TryGetValue(category, out var source)) return;

            AudioClip clip = GetClip(index);
            if (source.clip == clip && source.isPlaying)
            {
                if (fadeOutCoroutines.ContainsKey(category))
                {
                    StopCoroutine(fadeOutCoroutines[category]);
                }

                fadeOutCoroutines[category] = StartCoroutine(FadeOutAndStop(source, category, fadeTime));

                if (category == SECategory.Main)
                    isPlayingSE = false;
            }
        }

        private IEnumerator FadeOutAndStop(AudioSource source, SECategory category, float fadeTime)
        {
            float startVolume = source.volume;
            float timer = 0f;

            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                float t = timer / fadeTime;
                source.volume = Mathf.Lerp(startVolume, 0f, t);
                yield return null;
            }

            source.Stop();
            source.volume = startVolume;

            if (!loopingCategories.Contains(category))
            {
                source.clip = null;
                source.loop = false;
            }

            fadeOutCoroutines.Remove(category);
        }

        public void PlaySE_Force(int index, SECategory category = SECategory.Main)
        {
            if (!sourceMap.TryGetValue(category, out var source)) return;

            AudioClip clip = GetClip(index);
            if (clip != null)
            {
                StopSE(category);
                StartCoroutine(PlayAndWait(source, clip));
            }
        }

        private IEnumerator PlayAndWait(AudioSource source, AudioClip clip, Action onComplete = null)
        {
            isPlayingSE = true;
            source.PlayOneShot(clip);
            yield return new WaitForSecondsRealtime(clip.length);
            isPlayingSE = false;
            onComplete?.Invoke();
        }

        public void StopAllLoopSE()
        {
            foreach (var kv in sourceMap)
            {
                if (kv.Value.isPlaying && kv.Value.loop)
                {
                    kv.Value.Stop();
                    kv.Value.clip = null;
                    kv.Value.loop = false;
                }
            }
            isPlayingSE = false;
        }
    }
}