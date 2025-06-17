using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace kawanaka
{
    public class CameraGlitch : MonoBehaviour
    {
        public PostProcessVolume volume;

        private ChromaticAberration chromatic;
        private Grain grain;
        private float timer;

        void Start()
        {
            volume.profile.TryGetSettings(out chromatic);
            volume.profile.TryGetSettings(out grain);
        }

        public void TriggerGlitch(float duration)
        {
            StartCoroutine(Glitch(duration));
        }

        private IEnumerator Glitch(float duration)
        {
            if (chromatic != null) chromatic.intensity.value = 1f;
            if (grain != null) grain.intensity.value = 1f;

            yield return new WaitForSeconds(duration);

            if (chromatic != null) chromatic.intensity.value = 0f;
            if (grain != null) grain.intensity.value = 0f;
        }
    }
}