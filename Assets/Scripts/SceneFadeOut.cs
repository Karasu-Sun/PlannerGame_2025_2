using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFadeOut : MonoBehaviour
{
    public static SceneFadeOut Instance { get; private set; }

    public Image fadeImage;
    public float fadeDuration = 1.0f;

    private float fadeTimer = 0f;
    public bool isFading { get; private set; } = false;

    private System.Action onFadeComplete; // ’Ê’m—p

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitFadeImage();
        }
    }

    private void InitFadeImage()
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, 0f);
            fadeImage.raycastTarget = false;
        }
    }

    public void StartFadeOut(System.Action onComplete = null) // ˆø”
    {
        isFading = true;
        fadeTimer = 0f;

        this.onFadeComplete = onComplete; // •ÛŽ

        Color c = fadeImage.color;
        fadeImage.color = new Color(c.r, c.g, c.b, 0f);

        fadeImage.raycastTarget = true;
    }

    private void Update()
    {
        if (isFading)
        {
            Time.timeScale = 0f;
            FadeOut();
        }
    }

    private void FadeOut()
    {
        fadeTimer += Time.unscaledDeltaTime;

        float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);

        Color c = fadeImage.color;
        fadeImage.color = new Color(c.r, c.g, c.b, alpha);

        if (fadeTimer >= fadeDuration)
        {
            isFading = false;
            fadeImage.color = new Color(c.r, c.g, c.b, 1f);
            fadeImage.raycastTarget = false;

            Time.timeScale = 1f;

            onFadeComplete?.Invoke(); // ’Ê’m

            onFadeComplete = null;
        }
    }
}