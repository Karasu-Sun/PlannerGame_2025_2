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
                // �h���[���o�b�e���[�̎擾
                DroneBatterySystem drone = other.GetComponent<DroneBatterySystem>();

                // �e�L�X�g�\��
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