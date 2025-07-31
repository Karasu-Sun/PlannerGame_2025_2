using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class Key_Item : MonoBehaviour
    {
        [Header("Key_ID")]
        [SerializeField] private string requiredKeyID = "Door_Key";

        [Header("éQè∆")]
        [SerializeField] Key_Inventory key_Inventory;

        [SerializeField] private TypewriterText doorText;
        [SerializeField] private int getDoorKeyTextNum;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                key_Inventory.AddKey(requiredKeyID);
                doorText.StartTypingByIndex(getDoorKeyTextNum);
                Destroy(gameObject);
            }
        }
    }
}