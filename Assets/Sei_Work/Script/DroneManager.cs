using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class DroneManager : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;
        [SerializeField] private Camera subCamera;
        [SerializeField] private SEManager sEManager;
        [SerializeField] private GameObject player;

        private void Update()
        {
            bool isOperation = playerStatusManager.GetStatus(PlayerStatusType.IsOperation);
            if (!isOperation)
            {
                subCamera.transform.position = new Vector3(player.transform.position.x, subCamera.transform.position.y, player.transform.position.z);
            }
            subCamera.enabled = isOperation;
        }
    }
}