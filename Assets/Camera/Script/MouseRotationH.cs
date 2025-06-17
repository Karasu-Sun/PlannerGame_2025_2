using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class MouseRotationH : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f; // èâä˙âÒì]ë¨ìx

        [SerializeField] private float rotationY = 0f;
        [SerializeField] private PlayerStatusManager playerStatusManager;

        [SerializeField] private bool isOperating = false;
        [SerializeField] private bool isPausing = false;

        public void SetRotationSpeed(float newSpeed)
        {
            rotationSpeed = newSpeed;
        }

        private void Update()
        {
            isOperating = playerStatusManager.GetStatus(PlayerStatusType.IsOperation);
            isPausing = playerStatusManager.GetStatus(PlayerStatusType.IsOption);

            if (isOperating || isPausing) return;

            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            rotationY += mouseX;
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        }
    }
}