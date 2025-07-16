using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class CameraYFlipper : MonoBehaviour
    {
        [Header("回転対象のカメラ")]
        [SerializeField] private Transform cameraTransform;

        [Header("回転の速度")]
        [SerializeField] private float rotationSpeed = 5f;

        private bool isRotating = false;
        private Quaternion targetRotation;

        private void Update()
        {
            // キー入力(Z:仮)
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartFlip();
            }

            if (isRotating)
            {
                cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                // 近づいたら完了
                if (Quaternion.Angle(cameraTransform.rotation, targetRotation) < 0.1f)
                {
                    cameraTransform.rotation = targetRotation;
                    isRotating = false;
                }
            }
        }

        private void StartFlip()
        {
            // 180度回転させる
            Vector3 currentEuler = cameraTransform.rotation.eulerAngles;
            float newY = (currentEuler.y + 180f) % 360f;
            targetRotation = Quaternion.Euler(currentEuler.x, newY, currentEuler.z);

            isRotating = true;
        }
    }
}