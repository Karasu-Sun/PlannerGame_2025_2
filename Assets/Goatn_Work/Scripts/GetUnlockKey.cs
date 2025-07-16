using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetUnlockKey : MonoBehaviour
{
    // Œ®‚ÌŠ”
    public int keys;
    public TextMeshProUGUI keysUI;

    private void Start()
    {
        UpdateKeyUI();
    }

    private void OnTriggerStay(Collider other)
    {
        // Œ®‚Ìæ“¾
        if (other.gameObject.tag == "Key" && Input.GetKey(KeyCode.E))
        {
            // –¢”­Œ©‚Ìê‡‚Íæ“¾•s‰Â
            if (!other.gameObject.GetComponent<MeshRenderer>().enabled) return;

            keys++;
            UpdateKeyUI();
            
            Destroy(other.gameObject);
        }
    }

    public void UpdateKeyUI()
    {
        keysUI.text = "Key : " + keys;
    }
}
