using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class LightBattery : MonoBehaviour
    {
        [SerializeField] private float chargeAmount = 10f;

        [SerializeField] private TypewriterText typewriterText;
        [SerializeField] private int TextNum;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // ライトの取得
                LightSystem light = other.GetComponent<LightSystem>();

                // テキスト表示
                typewriterText.StartTypingByIndex(TextNum);

                if (light != null)
                {
                    light.ChargeBattery(chargeAmount);
                    Destroy(gameObject);
                }
            }
        }
    }
}