using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class StaminaUpgradeItem : MonoBehaviour
    {
        [Header("�X�^�~�i������")]
        [SerializeField] private float staminaIncreaseAmount = 20f;

        [SerializeField] private TypewriterText typewriterText;
        [SerializeField] private int TextNum;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // PlayerSprint �R���|�[�l���g���擾
                PlayerSprint playerSprint = other.GetComponent<PlayerSprint>();

                if (playerSprint != null)
                {
                    // �X�^�~�i�ő�l�𑝉�
                    playerSprint.IncreaseMaxStamina(staminaIncreaseAmount);

                    // �e�L�X�g�\��
                    typewriterText.StartTypingByIndex(TextNum);
                    Destroy(gameObject);
                }
            }
        }
    }
}