using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

namespace kawanaka
{
    public class BatteryPack : MonoBehaviour
    {
        [SerializeField] private float rechargeAmount = 30f;

        [SerializeField] private TypewriterText typewriterText;
        [SerializeField] private int TextNum;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // ドローンバッテリーの取得
                DroneBatterySystem drone = other.GetComponent<DroneBatterySystem>();

                // テキスト表示
                typewriterText.StartTypingByIndex(TextNum);

                if (drone != null)
                {
                    drone.RechargeBattery(rechargeAmount);
                    Destroy(gameObject);
                }
            }
        }
    }
}