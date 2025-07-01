using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace kawanaka
{
    public class VideoTransparencyController : MonoBehaviour
    {
        [SerializeField] private RawImage videoImage;

        [Range(0f, 1f)]
        public float alpha = 1f;

        private void Update()
        {
            if (videoImage != null)
            {
                Color color = videoImage.color;
                color.a = alpha;
                videoImage.color = color;
            }
        }
    }
}