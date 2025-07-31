using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class Key_Door : InteractableObject
    {
        [Header("必要な鍵ID")]
        public string requiredKeyID;

        [Header("ドア演出")]
        [SerializeField] private TypewriterText doorText;
        [SerializeField] private int doorCloseTextNum;
        [SerializeField] private int doorOpenTextNum;

        [Header("開閉オブジェクト")]
        [SerializeField] private Collider doorCollider;
        [SerializeField] private Transform doorToRotate;
        [SerializeField] private float openAngle = 80f;
        [SerializeField] private float rotationSpeed = 90f;

        private bool isOpened = false;
        private bool isRotating = false;
        private Quaternion targetRotation;

        public override void Interact(GameObject player)
        {
            if (isOpened) return;

            Key_Inventory inventory = player.GetComponent<Key_Inventory>();
            if (inventory != null && inventory.HasKey(requiredKeyID))
            {
                if (doorText != null)
                    doorText.StartTypingByIndex(doorOpenTextNum);

                isOpened = true;

                if (doorCollider != null)
                    doorCollider.enabled = false;

                if (doorToRotate != null)
                {
                    targetRotation = Quaternion.Euler(
                        doorToRotate.eulerAngles.x,
                        doorToRotate.eulerAngles.y + openAngle,
                        doorToRotate.eulerAngles.z
                    );
                    isRotating = true;
                }
            }
            else
            {
                if (doorText != null)
                    doorText.StartTypingByIndex(doorCloseTextNum);
            }
        }

        private void Update()
        {
            if (isRotating && doorToRotate != null)
            {
                doorToRotate.rotation = Quaternion.RotateTowards(
                    doorToRotate.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );

                if (Quaternion.Angle(doorToRotate.rotation, targetRotation) < 0.1f)
                {
                    doorToRotate.rotation = targetRotation;
                    isRotating = false;
                }
            }
        }
    }
}