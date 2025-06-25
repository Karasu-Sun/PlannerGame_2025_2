using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kawanaka; // ���l�[���X�y�[�X���Q�Ƃ���ꍇ�͂��ꂩ

namespace sei
{
    public class DroneManager : MonoBehaviour
    {
        [SerializeField] private kawanaka.PlayerStatusManager playerStatusManager; // �ꕔ�����Ȃ炱��ł��Q�Ƃł���
        [SerializeField] private Camera droneCamera;
        [SerializeField] private SEManager sEManager;
        [SerializeField] private GameObject player;

        private void Update()
        {
            bool isOperation = playerStatusManager.GetStatus(PlayerStatusType.IsOperation);
            if (!isOperation)
            {
                droneCamera.transform.position = new Vector3(player.transform.position.x, droneCamera.transform.position.y, player.transform.position.z);
            }
            droneCamera.enabled = isOperation;
        }
    }
}