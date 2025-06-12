using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class PlayerCenterFollower : MonoBehaviour
    {
        public enum FollowMode { Lerp, SmoothDamp, Spring }

        [Header("Target Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0, 0, -3);

        [Header("Follow Settings")]
        [SerializeField] private FollowMode mode = FollowMode.SmoothDamp;
        [SerializeField] private float followSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;

        [Header("Advanced")]
        [SerializeField] private float smoothTime = 0.3f;
        [SerializeField] private float springConstant = 16f;

        private Vector3 currentVelocity;
        private Vector3 springVelocity;

        private void FixedUpdate()
        {
            if (target == null) return;

            UpdatePosition();
            UpdateRotation();
        }

        private void UpdatePosition()
        {
            Vector3 targetPosition = target.TransformPoint(offset);

            switch (mode)
            {
                case FollowMode.Lerp:
                    transform.position = Vector3.Lerp(
                        transform.position,
                        targetPosition,
                        followSpeed * Time.deltaTime
                    );
                    break;

                case FollowMode.SmoothDamp:
                    transform.position = Vector3.SmoothDamp(
                        transform.position,
                        targetPosition,
                        ref currentVelocity,
                        smoothTime,
                        Mathf.Infinity,
                        Time.deltaTime
                    );
                    break;

                case FollowMode.Spring:
                    Vector3 displacement = targetPosition - transform.position;
                    springVelocity += displacement * (springConstant * Time.deltaTime);
                    springVelocity *= Mathf.Clamp01(1 - followSpeed * Time.deltaTime);
                    transform.position += springVelocity * Time.deltaTime;
                    break;
            }
        }

        private void UpdateRotation()
        {
            Quaternion targetRotation = Quaternion.LookRotation(
                target.position - transform.position,
                target.up
            );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        private void OnValidate()
        {
            if (target != null)
            {
                transform.position = target.TransformPoint(offset);
                transform.LookAt(target);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (target != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(target.TransformPoint(offset), 0.2f);
                Gizmos.DrawLine(transform.position, target.TransformPoint(offset));
            }
        }
#endif
    }
}