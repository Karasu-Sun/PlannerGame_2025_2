using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class InteractableObject : MonoBehaviour
    {
        [Header("メッセージ")]
        public string interactMessage = "何かがある...";

        public virtual void Interact()
        {
            Debug.Log($"{interactMessage}");
        }
    }
}