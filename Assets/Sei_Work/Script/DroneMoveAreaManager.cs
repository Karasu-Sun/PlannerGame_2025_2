using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using kawanaka;

namespace sei
{
    public class DroneMoveAreaManager : MonoBehaviour
    {
        [SerializeField] PlayerStatusManager playerStatusManager;
        [SerializeField] GameObject player;
        [SerializeField] GameObject center;
        [SerializeField] GameObject target;
        [SerializeField] float area;
 
        private void Update()
        {
            center.transform.position = new Vector3(player.transform.position.x, target.transform.position.y, player.transform.position.z);
            if (Vector3.Distance(target.transform.position, center.transform.position) > area)
            {
                playerStatusManager.SetStatus(PlayerStatusType.IsOperation, false);
            }
        }
    }
}