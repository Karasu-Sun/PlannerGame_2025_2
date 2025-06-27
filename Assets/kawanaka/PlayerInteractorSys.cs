using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class PlayerInteractorSys : MonoBehaviour
    {
        //public float interactRange = 5f;
        //public LayerMask interactableLayer;
        //public KeyCode interactKey = KeyCode.E;

        //[Header("Gizmo•\Ž¦")]
        //public bool showInteractGizmo = true;
        //public Color gizmoColor = Color.green;

        //[SerializeField] private PlayerStatusManager playerStatusManager;

        //private void Update()
        //{
        //    if (Input.GetKeyDown(interactKey))
        //    {
        //        TryInteract();
        //    }

        //    if (playerStatusManager != null && playerStatusManager.GetStatus(PlayerStatusType.IsInteracting))
        //    {
        //        LockToTargetPosition();
        //    }
        //}

        //public float interactAngle = 60f;

        //private void TryInteract()
        //{
        //    Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, interactableLayer);

        //    foreach (var hit in hits)
        //    {
        //        Vector3 toTarget = hit.transform.position - transform.position;
        //        float angle = Vector3.Angle(transform.forward, toTarget);

        //        if (angle <= interactAngle * 0.5f)
        //        {
        //            InteractableObject interactable = hit.GetComponent<InteractableObject>();
        //            if (interactable != null)
        //            {
        //                interactable.Interact(gameObject);
        //                break;
        //            }
        //        }
        //    }
        //}

        //private void LockToTargetPosition()
        //{
        //    Transform target = ;

        //    if (target != null)
        //    {
        //        Vector3 offset = -target.forward * 1.0f;
        //        transform.position = target.position + offset;

        //        Vector3 lookPos = target.position;
        //        lookPos.y = transform.position.y;
        //        transform.LookAt(lookPos);
        //    }
        //}

        //private void OnDrawGizmosSelected()
        //{
        //    if (!showInteractGizmo) return;

        //    Gizmos.color = gizmoColor;

        //    Gizmos.DrawWireSphere(transform.position, interactRange);

        //    Vector3 left = Quaternion.Euler(0, -interactAngle * 0.5f, 0) * transform.forward;
        //    Vector3 right = Quaternion.Euler(0, interactAngle * 0.5f, 0) * transform.forward;
        //    Gizmos.DrawLine(transform.position, transform.position + left * interactRange);
        //    Gizmos.DrawLine(transform.position, transform.position + right * interactRange);
        //}
    }
}