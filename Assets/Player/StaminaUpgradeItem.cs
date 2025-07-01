using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class StaminaUpgradeItem : MonoBehaviour
    {
        [Header("�X�^�~�i������")]
        [SerializeField] private float staminaIncreaseAmount = 20f;

        private void OnTriggerEnter(Collider other)
        {
            PlayerSprint playerSprint = other.GetComponent<PlayerSprint>();

            if (playerSprint != null)
            {
                // �X�^�~�i�ő�l�𑝂₷
                playerSprint.IncreaseMaxStamina(staminaIncreaseAmount);

                Destroy(gameObject);
            }
        }
    }
}