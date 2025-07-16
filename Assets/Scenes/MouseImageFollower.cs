using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    // Sys Code
    public class MouseImageFollower : MonoBehaviour
    {
        [Header("マウス画像")]
        [SerializeField] private RectTransform imageRectTransform;
        [SerializeField] private Canvas canvas;

        [Header("表示制御")]
        [SerializeField] private bool showImage;

        [Header("オフセット")]
        [SerializeField] private Vector2 offset = Vector2.zero;

        public static MouseImageFollower instance { get; private set; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            SetImageVisible(true);
        }

        private void Update()
        {
            if (!showImage)
            {
                imageRectTransform.gameObject.SetActive(false);
                return;
            }

            imageRectTransform.gameObject.SetActive(true);

            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out mousePos
            );

            imageRectTransform.anchoredPosition = mousePos + offset;
        }

        // 呼び出し
        public void SetImageVisible(bool visible)
        {
            showImage = visible;
        }
        public void SetOffset(Vector2 newOffset)
        {
            offset = newOffset;
        }
    }
}