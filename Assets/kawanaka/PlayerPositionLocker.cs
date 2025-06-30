using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class PlayerPositionLocker : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -1f);

        private void Update()
        {
            if (playerStatusManager != null && playerStatusManager.GetStatus(PlayerStatusType.IsInteracting))
            {
                LockToTargetPosition();
            }
        }

        [SerializeField] private float followSpeed = 5f;
        [SerializeField] private float rotateSpeed = 10f;

        private void LockToTargetPosition()
        {
            if (target != null)
            {
                Vector3 worldOffset = target.TransformDirection(offset);
                Vector3 targetPosition = target.position + worldOffset;

                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

                Vector3 lookPos = target.position;
                lookPos.y = transform.position.y;

                Quaternion targetRotation = Quaternion.LookRotation(lookPos - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}