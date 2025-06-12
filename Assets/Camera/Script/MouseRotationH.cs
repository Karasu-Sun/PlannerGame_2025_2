using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class MouseRotationH : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f; // ��]���x

        private float rotationY = 0f;

        void Update()
        {
            // �}�E�X�̈ړ��ʂ��擾
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;

            // Y����]�i���E�̓����j
            rotationY += mouseX;

            // ��]��K�p
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        }
    }
}