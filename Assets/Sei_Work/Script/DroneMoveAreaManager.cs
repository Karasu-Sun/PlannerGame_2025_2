using System.Collections;
using System.Collections.Generic;
using kawanaka;
using Unity.VisualScripting;
using UnityEngine;

public class DroneMoveAreaManager : MonoBehaviour
{
    [SerializeField] PlayerStatusManager playerStatusManager;
    [SerializeField] GameObject player;
    [SerializeField] GameObject center;
    [SerializeField] GameObject target;
    [SerializeField] float area;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        center.transform.position = new Vector3(player.transform.position.x, target.transform.position.y, player.transform.position.z);
        if(Vector3.Distance(target.transform.position,center.transform.position) > area)
        {
            playerStatusManager.SetStatus(PlayerStatusType.IsOperation, false);
        }
    }
}
