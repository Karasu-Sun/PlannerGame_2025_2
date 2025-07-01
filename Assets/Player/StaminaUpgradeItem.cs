using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class StaminaUpgradeItem : MonoBehaviour
    {
        [Header("スタミナ増加量")]
        [SerializeField] private float staminaIncreaseAmount = 20f;

        private void OnTriggerEnter(Collider other)
        {
            PlayerSprint playerSprint = other.GetComponent<PlayerSprint>();

            if (playerSprint != null)
            {
                // スタミナ最大値を増やす
                playerSprint.IncreaseMaxStamina(staminaIncreaseAmount);

                Destroy(gameObject);
            }
        }
    }
}