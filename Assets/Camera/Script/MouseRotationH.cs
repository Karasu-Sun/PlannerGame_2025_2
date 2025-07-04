using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class MouseRotationH : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f; // 初期回転速度

        [SerializeField] private float rotationY = 0f;
        [SerializeField] private PlayerStatusManager playerStatusManager;

        [SerializeField] private bool isOperating = false;
        [SerializeField] private bool isPausing = false;

        [SerializeField] private Transform playerModel;

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

            // 回転更新
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

            // モデルのY回転を同期
            if (playerModel != null)
            {
                Quaternion targetRot = Quaternion.Euler(playerModel.eulerAngles.x, rotationY, playerModel.eulerAngles.z);
                playerModel.rotation = Quaternion.Lerp(playerModel.rotation, targetRot, Time.deltaTime * 10f);
            }
        }
    }
}