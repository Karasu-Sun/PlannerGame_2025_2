using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;
        [SerializeField] private Camera subCamera;
        [SerializeField] private SEManager sEManager;

        private void Update()
        {
            bool isOperation = playerStatusManager.GetStatus(PlayerStatusType.IsOperation);
            subCamera.enabled = isOperation;
        }
    }
}