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
        [SerializeField] private TMP_Text MainSEVolumeText;
        [SerializeField] private TMP_Text StaminaSEVolumeText;
        [SerializeField] private TMP_Text SeriousSEVolumeText;
        [SerializeField] private TMP_Text SystemSEVolumeText;

        [SerializeField] private CanvasGroup volumePanel;

        [SerializeField] private Slider BGMvolumeSlider;
        [SerializeField] private Slider MainSESlider;
        [SerializeField] private Slider StaminaSESlider;
        [SerializeField] private Slider SeriousSESlider;
        [SerializeField] private Slider SystemSESlider;

        [Header("Audio Settings")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private string BGMvolumeParameter = "BGMVolume";
        [SerializeField] private string Main_SEvolumeParameter = "Main_SE";
        [SerializeField] private string Stamina_SEvolumeParameter = "Stamina_SE";
        [SerializeField] private string Serious_SEvolumeParameter = "Serious_SE";
        [SerializeField] private string System_SEvolumeParameter = "System_SE";

        private float targetAlpha = 0f;
        [SerializeField] private float fadeSpeed = 5f;

        private void Start()
        {
            LoadVolumeSettings();
            UpdateVolumeDisplay();

            BGMvolumeSlider.onValueChanged.AddListener(value => SetVolume(BGMvolumeParameter, value, BGMVolumeText, "BGM", "BGMVolume"));
            MainSESlider.onValueChanged.AddListener(value => SetVolume(Main_SEvolumeParameter, value, MainSEVolumeText, "Main", "MainSEVolume"));
            StaminaSESlider.onValueChanged.AddListener(value => SetVolume(Stamina_SEvolumeParameter, value, StaminaSEVolumeText, "Stamina", "StaminaSEVolume"));
            SeriousSESlider.onValueChanged.AddListener(value => SetVolume(Serious_SEvolumeParameter, value, SeriousSEVolumeText, "Serious", "SeriousSEVolume"));
            SystemSESlider.onValueChanged.AddListener(value => SetVolume(System_SEvolumeParameter, value, SystemSEVolumeText, "System", "SystemSEVolume"));

            DisableKeyboardInputForSlider(BGMvolumeSlider);
            DisableKeyboardInputForSlider(MainSESlider);
            DisableKeyboardInputForSlider(StaminaSESlider);
            DisableKeyboardInputForSlider(SeriousSESlider);
            DisableKeyboardInputForSlider(SystemSESlider);
        }

        private void Update()
        {
            if (volumePanel == null) return;

            volumePanel.alpha = Mathf.MoveTowards(volumePanel.alpha, targetAlpha, Time.unscaledDeltaTime * fadeSpeed);

            bool isVisible = volumePanel.alpha > 0.01f;
            volumePanel.interactable = isVisible;
            volumePanel.blocksRaycasts = isVisible;
        }

        private void SetVolume(string parameter, float value, TMP_Text displayText, string label, string prefsKey)
        {
            float dB = value > 0 ? 20f * Mathf.Log10(value) : -80f;

            if (audioMixer != null && !audioMixer.SetFloat(parameter, dB))
                Debug.LogError($"{parameter} Ç™ë∂ç›ÇµÇ‹ÇπÇÒ");

            if (displayText != null)
                displayText.text = $"{label}: {Mathf.RoundToInt(value * 100)}%";

            PlayerPrefs.SetFloat(prefsKey, value);
        }

        private void LoadVolumeSettings()
        {
            SetSliderAndMixer(BGMvolumeSlider, BGMvolumeParameter, "BGMVolume");
            SetSliderAndMixer(MainSESlider, Main_SEvolumeParameter, "MainSEVolume");
            SetSliderAndMixer(StaminaSESlider, Stamina_SEvolumeParameter, "StaminaSEVolume");
            SetSliderAndMixer(SeriousSESlider, Serious_SEvolumeParameter, "SeriousSEVolume");
            SetSliderAndMixer(SystemSESlider, System_SEvolumeParameter, "SystemSEVolume");
        }

        private void SetSliderAndMixer(Slider slider, string mixerParam, string prefsKey)
        {
            float volume = PlayerPrefs.GetFloat(prefsKey, 0.5f);
            if (slider != null)
            {
                slider.value = volume;
                float dB = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
                if (!audioMixer.SetFloat(mixerParam, dB))
                    Debug.LogError($"{mixerParam} Ç™ë∂ç›ÇµÇ‹ÇπÇÒ");
            }
        }

        private void UpdateVolumeDisplay()
        {
            BGMVolumeText.text = $"BGM: {Mathf.RoundToInt(BGMvolumeSlider.value * 100)}%";
            MainSEVolumeText.text = $"Main: {Mathf.RoundToInt(MainSESlider.value * 100)}%";
            StaminaSEVolumeText.text = $"Stamina: {Mathf.RoundToInt(StaminaSESlider.value * 100)}%";
            SeriousSEVolumeText.text = $"Serious: {Mathf.RoundToInt(SeriousSESlider.value * 100)}%";
            SystemSEVolumeText.text = $"System: {Mathf.RoundToInt(SystemSESlider.value * 100)}%";
        }

        public void ToggleVolumePanel()
        {
            if (volumePanel == null) return;
            targetAlpha = (targetAlpha == 0f) ? 1f : 0f;
        }

        private void DisableKeyboardInputForSlider(Slider slider)
        {
            if (slider == null) return;
            slider.navigation = new Navigation { mode = Navigation.Mode.None };
        }
    }
}