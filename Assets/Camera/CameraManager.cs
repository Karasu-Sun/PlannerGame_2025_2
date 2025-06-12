using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;
        [SerializeField] private Camera subCamera;

        private void Update()
        {
            if (playerStatusManager == null || subCamera == null) return;

            bool isOperation = playerStatusManager.GetStatus(PlayerStatusType.IsOperation);

            // サブカメラを有効化／無効化
            subCamera.enabled = isOperation;
        }
    }
}