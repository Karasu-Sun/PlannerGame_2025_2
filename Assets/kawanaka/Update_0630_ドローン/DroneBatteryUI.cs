using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace kawanaka
{
    public class DroneBatteryUI : MonoBehaviour
    {
        [SerializeField] private DroneBatterySystem batterySystem;
        [SerializeField] private Slider batterySlider;

        private void Start()
        {
            if (batterySystem != null && batterySlider != null)
            {
                batterySlider.maxValue = 100f;
            }
        }

        private void Update()
        {
            if (batterySystem != null && batterySlider != null)
            {
                batterySlider.value = batterySystem.Battery;
            }
        }
    }
}