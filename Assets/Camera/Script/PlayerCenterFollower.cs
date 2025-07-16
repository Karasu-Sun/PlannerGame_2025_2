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

        [Header("Advanced")]
        [SerializeField] private float smoothTime = 0.3f;
        [SerializeField] private float springConstant = 16f;

        [Header("Vertical Bob Settings")]
        [SerializeField] private PlayerStatusManager playerStatusManager;
        [SerializeField] private PlayerStatusType sprintStatus = PlayerStatusType.IsSprint;
        [SerializeField] private PlayerStatusType walkStatus = PlayerStatusType.IsWalk;
        [SerializeField] private PlayerStatusType crouchStatus = PlayerStatusType.IsCrouch;

        [SerializeField] private float sprintBobAmplitude = 0.1f;
        [SerializeField] private float sprintBobFrequency = 10f;
        [SerializeField] private float walkBobAmplitude = 0.06f;
        [SerializeField] private float walkBobFrequency = 6f;
        [SerializeField] private float crouchBobAmplitude = 0.03f;
        [SerializeField] private float crouchBobFrequency = 4f;

        private Vector3 currentVelocity;
        private Vector3 springVelocity;

        private float bobTimer = 0f;
        private float currentBobOffset = 0f;

        private void LateUpdate()
        {
            if (target == null || playerStatusManager == null) return;
            UpdatePositionWithBob();
        }

        private void UpdatePositionWithBob()
        {
            Vector3 targetPosition = target.TransformPoint(offset);

            // Šî–{’Ç]ŒvZ
            Vector3 followPos = transform.position;

            switch (mode)
            {
                case FollowMode.Lerp:
                    followPos = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
                    break;
                case FollowMode.SmoothDamp:
                    followPos = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime, Mathf.Infinity, Time.deltaTime);
                    break;
                case FollowMode.Spring:
                    Vector3 displacement = targetPosition - transform.position;
                    springVelocity += displacement * (springConstant * Time.deltaTime);
                    springVelocity *= Mathf.Clamp01(1 - followSpeed * Time.deltaTime);
                    followPos += springVelocity * Time.deltaTime;
                    break;
            }

            // c—h‚êiBobjŒvZ
            bool isSprinting = playerStatusManager.GetStatus(sprintStatus);
            bool isWalking = playerStatusManager.GetStatus(walkStatus);
            bool isCrouching = playerStatusManager.GetStatus(crouchStatus);

            float amplitude = 0f;
            float frequency = 0f;

            if (isSprinting)
            {
                amplitude = sprintBobAmplitude;
                frequency = sprintBobFrequency;
            }
            else if (isWalking)
            {
                amplitude = walkBobAmplitude;
                frequency = walkBobFrequency;
            }
            else if (isCrouching)
            {
                amplitude = crouchBobAmplitude;
                frequency = crouchBobFrequency;
            }

            if (amplitude > 0f && frequency > 0f)
            {
                bobTimer += Time.deltaTime * frequency;
                currentBobOffset = Mathf.Sin(bobTimer) * amplitude;
            }
            else
            {
                bobTimer = 0f;
                currentBobOffset = Mathf.Lerp(currentBobOffset, 0f, Time.deltaTime * 10f);
            }

            // ÅIˆÊ’u‚É—h‚ê¬•ª‚ğ‰ÁZ‚µ‚Äİ’è
            transform.position = followPos + new Vector3(0, currentBobOffset, 0);
        }
    }
}