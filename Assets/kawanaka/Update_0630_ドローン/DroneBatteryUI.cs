using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace kawanaka
{
    public class DroneBatteryUI : MonoBehaviour
    // Old
    //{
    //    [SerializeField] private DroneBatterySystem batterySystem;
    //    [SerializeField] private Slider batterySlider;

    //    private void Start()
    //    {
    //        if (batterySystem != null && batterySlider != null)
    //        {
    //            batterySlider.maxValue = 100f;
    //        }
    //    }

    //    private void Update()
    //    {
    //        if (batterySystem != null && batterySlider != null)
    //        {
    //            batterySlider.value = batterySystem.Battery;
    //        }
    //    }
    //}

    {
        [SerializeField] private DroneBatterySystem batterySystem;
        [SerializeField] private Image fillImage; // íÜÇÃè[ìdó ï\é¶ïîï™

        private void Update()
        {
            if (batterySystem == null || fillImage == null) return;

            fillImage.fillAmount = batterySystem.Battery / 100f;
        }
    }
}