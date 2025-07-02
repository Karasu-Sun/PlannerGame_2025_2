using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class StaminaUpgradeItem : MonoBehaviour
    {
        [Header("スタミナ増加量")]
        [SerializeField] private float staminaIncreaseAmount = 20f;

        [SerializeField] private TypewriterText typewriterText;
        [SerializeField] private int TextNum;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // PlayerSprint コンポーネントを取得
                PlayerSprint playerSprint = other.GetComponent<PlayerSprint>();

                if (playerSprint != null)
                {
                    // スタミナ最大値を増加
                    playerSprint.IncreaseMaxStamina(staminaIncreaseAmount);

                    // テキスト表示
                    typewriterText.StartTypingByIndex(TextNum);
                    Destroy(gameObject);
                }
            }
        }
    }
}