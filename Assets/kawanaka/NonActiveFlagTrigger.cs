using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

public class NonActiveFlagTrigger : MonoBehaviour
{
    [SerializeField] public bool nonActiveFlag = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nonActiveFlag = true;
        }
    }
}
