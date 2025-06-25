using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class PlayerInteractor : MonoBehaviour
    {
        public float interactRange = 2f;
        public LayerMask interactableLayer;

        [SerializeField] public bool interact = false;

        private Camera playerCamera;

        private void Start()
        {
            playerCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryInteract();
            }
        }

        private void TryInteract()
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
            {
                InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
                if (interactable != null)
                {
                    interact = !interact;
                    interactable.Interact();
                }
            }
        }
    }
}