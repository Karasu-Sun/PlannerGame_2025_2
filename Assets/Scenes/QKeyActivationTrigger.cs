using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class QKeyActivationTrigger : MonoBehaviour
    {
        public bool isActivated = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isActivated = true;

                //Destroy(gameObject);
            }
        }
    }
}