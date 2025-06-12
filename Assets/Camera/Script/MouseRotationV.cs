using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class MouseRotationV : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f; // ��]���x

        private float rotationX = 0f;

        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            // X���i�㉺�̓����j
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f); // ��]�͈͂𐧌�

            // ���݂�Y����]���ێ�
            float currentY = transform.rotation.eulerAngles.y;

            // ��]��K�p
            transform.rotation = Quaternion.Euler(rotationX, currentY, 0f);
        }
    }
}