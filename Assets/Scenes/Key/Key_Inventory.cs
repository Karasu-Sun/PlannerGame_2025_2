using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class Key_Inventory : MonoBehaviour
    {
        private HashSet<string> keys = new HashSet<string>();

        public void AddKey(string keyID)
        {
            keys.Add(keyID);
        }

        public bool HasKey(string keyID)
        {
            return keys.Contains(keyID);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.K))
            {
                DebugPrintKeys();
            }
#endif
        }

        public void DebugPrintKeys()
        {
            if (keys.Count == 0)
            {
                Debug.Log("Œ®‚ğ1‚Â‚àŠ‚µ‚Ä‚¢‚Ü‚¹‚ñB");
                return;
            }

            Debug.Log("Œ»İŠ‚µ‚Ä‚¢‚éŒ®:");
            foreach (string key in keys)
            {
                Debug.Log($"- {key}");
            }
        }
    }
}