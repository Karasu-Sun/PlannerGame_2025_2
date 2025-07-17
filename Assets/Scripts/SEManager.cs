using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class SEManager : MonoBehaviour
    {
        //=========================
        // サウンドカテゴリ定義
        //=========================
        public enum SECategory
        {
            Main,        // 主に使われる汎用効果音
            Stamina,     // スタミナ関連の効果音
            Serious,     // 緊張感・重要イベント
            Effect,      // 特殊効果系
            System,      // UIやシステム操作音
            BgmLike,     // BGM的な音扱い
            Environment,  // 環境音など
            Footsteps,   // 足音系
            Drone       // ドローン系
        }

        [System.Serializable]
        public class AudioCategorySource
        {
            [Tooltip("カテゴリ種別")]
            public SECategory category;

            [Tooltip("再生に使うAudioSource")]
            public AudioSource source;
        }

        //=========================
        // フィールド
        //=========================

        public static SEManager Instance { get; private set; }

        [Header("SE音源クリップ群")]
        [Tooltip("インデックス指定や名前指定で再生するSE素材群")]
        [SerializeField] private AudioClip[] soundEffects;

        [Header("カテゴリごとのAudioSource設定")]
        [Tooltip("各カテゴリに対応するAudioSourceを割り当てる")]
        [SerializeField] private List<AudioCategorySource> categorySources = new List<AudioCategorySource>();

        // 内部用のAudioSourceマップ
        private Dictionary<SECategory, AudioSource> sourceMap = new Dictionary<SECategory, AudioSource>();

        [Header("PlaySE_Blocking 状態保持")]
        [Tooltip("ブロッキング再生中フラグ（再生中に他のSEをブロック）")]
        [SerializeField] private bool isPlayingSE = false;
        public bool IsPlayingSE => isPlayingSE;

        // ループ再生中のカテゴリ記録
        private HashSet<SECategory> loopingCategories = new HashSet<SECategory>();

        // フェードアウト処理中のコルーチン記録
        private Dictionary<SECategory, Coroutine> fadeOutCoroutines = new Dictionary<SECategory, Coroutine>();

        //=========================
        // 初期化処理
        //=========================
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

        // カテゴリごとのAudioSourceマッピングを初期化
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

        //=========================
        // SEの取得
        //=========================
        public AudioClip GetClip(int index)
        {
            if (index >= 0 && index < soundEffects.Length)
            {
                return soundEffects[index];
            }

            Debug.LogWarning($"SE Clipが見つかりません: Index {index}");
            return null;
        }

        //=========================
        // SEの再生（通常）
        //=========================
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

        //=========================
        // SEの再生（ブロッキング）
        //=========================
        public void PlaySE_Blocking(string name, SECategory category = SECategory.Main)
        {
            if (isPlayingSE) return;

            AudioClip clip = Array.Find(soundEffects, se => se.name == name);
            if (clip != null && sourceMap.TryGetValue(category, out var source))
            {
                StartCoroutine(PlayAndWait(source, clip));
            }
            else
            {
                Debug.LogWarning($"SEが見つかりません: {name}");
            }
        }

        public void PlaySE_Blocking(int index, SECategory category = SECategory.Main)
        {
            if (isPlayingSE) return;

            AudioClip clip = GetClip(index);
            if (clip != null && sourceMap.TryGetValue(category, out var source))
            {
                StartCoroutine(PlayAndWait(source, clip));
            }
            else
            {
                Debug.LogWarning($"SEが見つかりません: Index {index}");
            }
        }

        //=========================
        // SEの再生（ループ）
        //=========================
        public void PlaySE_Looping(int index, SECategory category = SECategory.Main)
        {
            if (!sourceMap.TryGetValue(category, out var source)) return;

            AudioClip clip = GetClip(index);
            if (clip == null) return;

            if (source.clip != clip)
            {
                source.clip = clip;
                source.loop = true;

                source.volume = 1.0f;

                source.Play();

                loopingCategories.Add(category);
            }
        }

        // SEの再生（ループ）
        public void FadeOutAndPlaySE_Looping(int nextIndex, SECategory category = SECategory.Main, float fadeTime = 0.3f)
        {
            if (!sourceMap.TryGetValue(category, out var source)) return;

            AudioClip nextClip = GetClip(nextIndex);
            if (nextClip == null) return;

            if (fadeOutCoroutines.ContainsKey(category))
                StopCoroutine(fadeOutCoroutines[category]);

            fadeOutCoroutines[category] = StartCoroutine(FadeOutThenPlayNext(source, nextClip, category, fadeTime));
        }

        private IEnumerator FadeOutThenPlayNext(AudioSource source, AudioClip nextClip, SECategory category, float fadeTime)
        {
            float startVolume = source.volume;
            float timer = 0f;

            // フェードアウト
            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                float t = timer / fadeTime;
                source.volume = Mathf.Lerp(startVolume, 0f, t);
                yield return null;
            }

            source.Stop();
            source.clip = nextClip;
            source.loop = true;
            source.volume = 0f;
            source.Play();

            loopingCategories.Add(category);

            // フェードイン
            yield return FadeIn(source, 1.0f, fadeTime);

            fadeOutCoroutines.Remove(category);
        }

        private IEnumerator FadeIn(AudioSource source, float targetVolume, float time)
        {
            float timer = 0f;
            float start = source.volume;

            while (timer < time)
            {
                timer += Time.deltaTime;
                float t = timer / time;
                source.volume = Mathf.Lerp(start, targetVolume, t);
                yield return null;
            }

            source.volume = targetVolume;
        }

        //=========================
        // SEの停止
        //=========================
        public void StopSE(SECategory category = SECategory.Main, float fadeTime = 0.5f)
        {
            if (!sourceMap.TryGetValue(category, out var source)) return;

            if (source.isPlaying)
            {
                if (fadeOutCoroutines.ContainsKey(category))
                    StopCoroutine(fadeOutCoroutines[category]);

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
                    StopCoroutine(fadeOutCoroutines[category]);

                fadeOutCoroutines[category] = StartCoroutine(FadeOutAndStop(source, category, fadeTime));

                if (category == SECategory.Main)
                    isPlayingSE = false;
            }
        }

        //=========================
        // 3D位置でのSE再生
        //=========================
        public void PlaySEAtPosition(int index, Vector3 position, SECategory category = SECategory.Main, float volume = 1f)
        {
            AudioClip clip = GetClip(index);
            if (clip == null) return;

            GameObject tempGO = new GameObject($"SE_{clip.name}_3D");
            tempGO.transform.position = position;

            AudioSource source = tempGO.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = 1.0f;
            source.volume = volume;
            source.Play();

            Destroy(tempGO, clip.length);
        }

        public AudioSource PlaySELoopAtPosition(int index, Vector3 position, SECategory category = SECategory.Main)
        {
            AudioClip clip = GetClip(index);
            if (clip == null) return null;

            GameObject loopGO = new GameObject($"SE_{clip.name}_Loop");
            loopGO.transform.position = position;

            AudioSource source = loopGO.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = true;
            source.spatialBlend = 1.0f;
            source.Play();

            return source;
        }

        //=========================
        // 再生中SEのフェードアウト停止
        //=========================
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

        //=========================
        // 強制再生（停止→再生）
        //=========================
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

        //=========================
        // SEの再生完了待機コルーチン
        //=========================
        private IEnumerator PlayAndWait(AudioSource source, AudioClip clip, Action onComplete = null)
        {
            isPlayingSE = true;
            source.PlayOneShot(clip);
            yield return new WaitForSecondsRealtime(clip.length);
            isPlayingSE = false;
            onComplete?.Invoke();
        }

        //=========================
        // すべてのループSEを停止
        //=========================
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