using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance { get; private set; }

    public Image fadeImage;
    public float fadeDuration = 1.0f;

    private float fadeTimer = 0f;
    public bool isFading { get; private set; } = false;

    private System.Action onFadeComplete;

    private enum FadeType { None, In, Out }
    private FadeType currentFade = FadeType.None;

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

    public void StartFadeOut(System.Action onComplete = null)
    {
        isFading = true;
        currentFade = FadeType.Out;
        fadeTimer = 0f;
        onFadeComplete = onComplete;

        Color c = fadeImage.color;
        fadeImage.color = new Color(c.r, c.g, c.b, 0f);
        fadeImage.raycastTarget = true;
    }

    public void StartFadeIn(System.Action onComplete = null)
    {
        isFading = true;
        currentFade = FadeType.In;
        fadeTimer = 0f;
        onFadeComplete = onComplete;

        Color c = fadeImage.color;
        fadeImage.color = new Color(c.r, c.g, c.b, 1f);
        fadeImage.raycastTarget = true;
    }

    private void Update()
    {
        if (!isFading) return;

        Time.timeScale = 0f;

        switch (currentFade)
        {
            case FadeType.Out:
                FadeOut();
                break;
            case FadeType.In:
                FadeIn();
                break;
        }
    }

    private void FadeOut()
    {
        fadeTimer += Time.unscaledDeltaTime;

        float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);

        SetFadeAlpha(alpha);

        if (fadeTimer >= fadeDuration)
            EndFade(1f); // Š®‘S‚É•
    }

    private void FadeIn()
    {
        fadeTimer += Time.unscaledDeltaTime;

        float alpha = Mathf.Clamp01(1f - (fadeTimer / fadeDuration));

        SetFadeAlpha(alpha);

        if (fadeTimer >= fadeDuration)
            EndFade(0f); // Š®‘S‚É“§–¾
    }

    private void SetFadeAlpha(float alpha)
    {
        Color c = fadeImage.color;
        fadeImage.color = new Color(c.r, c.g, c.b, alpha);
    }

    private void EndFade(float finalAlpha)
    {
        isFading = false;
        SetFadeAlpha(finalAlpha);
        fadeImage.raycastTarget = false;
        Time.timeScale = 1f;
        onFadeComplete?.Invoke();
        onFadeComplete = null;
        currentFade = FadeType.None;
    }
}