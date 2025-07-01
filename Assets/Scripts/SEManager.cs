using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class SEManager : MonoBehaviour
    {
        public static SEManager Instance { get; private set; }

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] soundEffects;

        [SerializeField] private bool isPlayingSE = false;
        public bool IsPlayingSE => isPlayingSE;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public AudioClip GetClip(int index)
        {
            if (index >= 0 && index < soundEffects.Length)
            {
                return soundEffects[index];
            }

            Debug.LogWarning($"SE Clip��������܂���: Index {index}");
            return null;
        }

        // �C���f�b�N�X�w���SE���Đ�����
        public void PlaySE(int index)
        {
            if (index >= 0 && index < soundEffects.Length && soundEffects[index] != null)
            {
                audioSource.PlayOneShot(soundEffects[index]);
            }
            else
            {
                Debug.LogWarning($"SE��������܂���: Index {index}");
            }
        }

        // ���O�w���SE���Đ�����
        public void PlaySE(string name)
        {
            AudioClip clip = Array.Find(soundEffects, se => se.name == name);
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
            else
            {
                Debug.LogWarning($"SE��������܂���: {name}");
            }
        }

        // ���O�w��F�Đ����͖���Ver
        public void PlaySE_Blocking(string name)
        {
            if (isPlayingSE) return;

            AudioClip clip = Array.Find(soundEffects, se => se.name == name);
            if (clip != null)
            {
                StartCoroutine(PlayAndWait(clip));
            }
            else
            {
                Debug.LogWarning($"SE��������܂���: {name}");
            }
        }

        // �C���f�b�N�X�w��F�Đ����͖���Ver
        public void PlaySE_Blocking(int index)
        {
            if (isPlayingSE) return;

            if (index >= 0 && index < soundEffects.Length && soundEffects[index] != null)
            {
                StartCoroutine(PlayAndWait(soundEffects[index]));
            }
            else
            {
                Debug.LogWarning($"SE��������܂���: Index {index}");
            }
        }
        public void PlaySE_Looping(int index)
        {
            if (audioSource.clip != soundEffects[index])
            {
                audioSource.clip = soundEffects[index];
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        public void StopSE()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                isPlayingSE = false;
            }
        }

        public void PlaySE_Force(int index)
        {
            if (index >= 0 && index < soundEffects.Length && soundEffects[index] != null)
            {
                StopSE();
                StartCoroutine(PlayAndWait(soundEffects[index]));
            }
            else
            {
                Debug.LogWarning($"SE��������܂���: Index {index}");
            }
        }

        private IEnumerator PlayAndWait(AudioClip clip, Action onComplete = null)
        {
            isPlayingSE = true;
            audioSource.PlayOneShot(clip);
            yield return new WaitForSecondsRealtime(clip.length);
            isPlayingSE = false;
            onComplete?.Invoke();
        }
    }
}