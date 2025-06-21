using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kawanaka; // 他ネームスペースを参照する場合はこれか

namespace sei
{
    public class DroneManager : MonoBehaviour
    {
        [SerializeField] private kawanaka.PlayerStatusManager playerStatusManager; // 一部だけならこれでも参照できる
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