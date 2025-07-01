using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

namespace kawanaka
{
    public class BatteryPack : MonoBehaviour
    {
        [SerializeField] private float rechargeAmount = 30f;

        private void OnTriggerEnter(Collider other)
        {
            DroneBatterySystem drone = other.GetComponent<DroneBatterySystem>();
            if (drone != null)
            {
                drone.RechargeBattery(rechargeAmount);
                Destroy(gameObject);
            }
        }
    }
}