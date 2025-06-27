using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class InteractableObject : MonoBehaviour
    {
        [Header("���b�Z�[�W")]
        public string interactMessage = "����������...";

        public virtual void Interact()
        {
            Debug.Log($"{interactMessage}");
        }
    }
}