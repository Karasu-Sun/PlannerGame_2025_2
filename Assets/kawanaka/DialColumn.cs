using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class DialColumn : MonoBehaviour
    {
        [SerializeField] private int currentValue = 0;
        public int CurrentValue => currentValue;

        private float rotationStep = 36f; // 360�x / 10���l = 36�x����
        private Quaternion targetRotation;
        private float rotationSpeed = 10f;

        private bool isDragging = false;
        private float dragStartY;

        [SerializeField] private float transferCoefficient = 200.0f;

        [SerializeField] private DialLockManager dialLockManager;

        private void Start()
        {
            targetRotation = Quaternion.Euler(currentValue * -rotationStep, 0, 0);
            transform.localRotation = targetRotation;
        }

        private void Update()
        {
            // ��ԉ�]
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        private void OnMouseDown()
        {
            isDragging = true;
            dragStartY = Input.mousePosition.y;
        }

        private void OnMouseUp()
        {
            isDragging = false;
        }

        private void OnMouseDrag()
        {
            if (!isDragging || dialLockManager.isUnlocked) return;

            float dragDelta = Input.mousePosition.y - dragStartY;

            if (Mathf.Abs(dragDelta) > transferCoefficient)
            {
                if (dragDelta > 0)
                {
                    RotateDial(-1); // ��h���b�O �� ������
                    SEManager.Instance.PlaySE(7);
                }
                else
                {
                    RotateDial(1);  // ���h���b�O �� ������
                    SEManager.Instance.PlaySE(7);
                }

            dragStartY = Input.mousePosition.y;
            }
        }

        private void RotateDial(int direction)
        {
            currentValue = (currentValue + direction + 10) % 10;
            targetRotation = Quaternion.Euler(currentValue * -rotationStep, 0, 0);
        }
    }
}