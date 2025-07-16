using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoverKey : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Key")
        {
            // ŒõŒ¹‚É“ü‚Á‚½Œ®‚ð‰ÂŽ‹‰»
            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
