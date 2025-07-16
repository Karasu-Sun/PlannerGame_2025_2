using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace kawanaka
{
    public class VolumeControl : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] private TMP_Text BGMVolumeText;
        [SerializeField] private TMP_Text SEVolumeText;
        [SerializeField] private CanvasGroup volumePanel;
        [SerializeField] private Slider BGMvolumeSlider;
        [SerializeField] private Slider SEvolumeSlider;

        [Header("Audio Settings")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private string BGMvolumeParameter = "BGMVolume";
        [SerializeField] private string SEvolumeParameter = "SEVolume";

        private float BGMVolume = 0.5f;
        private float SEVolume = 0.5f;
        private float targetAlpha = 0f;

        [SerializeField] private float fadeSpeed = 5f;

        private void Start()
        {
            LoadVolumeSettings();
            UpdateVolumeDisplay();

            if (BGMvolumeSlider != null)
            {
                BGMvolumeSlider.onValueChanged.AddListener(SetBGMVolume);
                BGMvolumeSlider.interactable = true;
            }

            if (SEvolumeSlider != null)
            {
                SEvolumeSlider.onValueChanged.AddListener(SetSEVolume);
                SEvolumeSlider.interactable = true;
            }

            DisableKeyboardInputForSlider(SEvolumeSlider);
            DisableKeyboardInputForSlider(BGMvolumeSlider);
        }

        private void Update()
        {
            if (volumePanel == null) return;

            volumePanel.alpha = Mathf.MoveTowards(volumePanel.alpha, targetAlpha, Time.unscaledDeltaTime * fadeSpeed);

            bool isVisible = volumePanel.alpha > 0.01f;
            volumePanel.interactable = isVisible;
            volumePanel.blocksRaycasts = isVisible;

            UpdatePanelVisibility();
        }

        public void SetBGMVolume(float value)
        {
            BGMVolume = value;
            SetVolume(BGMvolumeParameter, BGMVolume);
            UpdateVolumeDisplay();
            SaveVolumeSettings();
        }

        public void SetSEVolume(float value)
        {
            SEVolume = value;
            SetVolume(SEvolumeParameter, SEVolume);
            UpdateVolumeDisplay();
        }

        private void SetVolume(string parameter, float volume)
        {
            float dB = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;

            if (audioMixer != null)
            {
                if (!audioMixer.SetFloat(parameter, dB))
                    Debug.LogError($"{parameter} Ç™ë∂ç›ÇµÇ‹ÇπÇÒ");
            }
        }

        private void UpdateVolumeDisplay()
        {
            int BGMPercentage = Mathf.RoundToInt(BGMVolume * 100);
            int SEPercentage = Mathf.RoundToInt(SEVolume * 100);

            BGMVolumeText.text = $"BGM: {BGMPercentage}%";
            SEVolumeText.text = $"SE: {SEPercentage}%";

            if (BGMvolumeSlider != null)
                BGMvolumeSlider.value = BGMVolume;

            if (SEvolumeSlider != null)
                SEvolumeSlider.value = SEVolume;
        }

        private void LoadVolumeSettings()
        {
            BGMVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
            SEVolume = PlayerPrefs.GetFloat("SEVolume", 0.5f);

            SetVolume(BGMvolumeParameter, BGMVolume);
            SetVolume(SEvolumeParameter, SEVolume);
        }

        private void SaveVolumeSettings()
        {
            PlayerPrefs.SetFloat("BGMVolume", BGMVolume);
            PlayerPrefs.SetFloat("SEVolume", SEVolume);
            PlayerPrefs.Save();
        }

        public void ToggleVolumePanel()
        {
            if (volumePanel == null) return;

            bool shouldShow = volumePanel.alpha == 0f;
            targetAlpha = shouldShow ? 1f : 0f;
        }

        private void UpdatePanelVisibility()
        {
            if (volumePanel != null)
            {
                volumePanel.alpha = targetAlpha;

                volumePanel.interactable = (targetAlpha == 1f);
                volumePanel.blocksRaycasts = (targetAlpha == 1f);
            }
        }

        private void DisableKeyboardInputForSlider(Slider slider)
        {
            slider.navigation = new Navigation
            {
                mode = Navigation.Mode.None
            };
        }
    }
}