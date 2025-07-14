using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class CameraCrouchAdjuster : MonoBehaviour
    {
        [Header("éQè∆ê›íË")]
        [SerializeField] private PlayerStatusManager playerStatusManager;
        [SerializeField] private Transform cameraTransform;

        [Header("à íuï‚ê≥ê›íË")]
        [SerializeField] private float crouchYOffset = -0.5f;

        [Header("à⁄ìÆÇÃääÇÁÇ©Ç≥")]
        [SerializeField] private float smoothSpeed = 8f;

        private Vector3 originalCameraLocalPos;
        private float targetYOffset = 0f;

        private void Start()
        {
            if (cameraTransform != null)
            {
                originalCameraLocalPos = cameraTransform.localPosition;
            }
            else
            {
                Debug.LogWarning("CameraTransform Ç™ñ¢ê›íËÇ≈Ç∑", this);
            }
        }

        private void Update()
        {
            if (playerStatusManager == null || cameraTransform == null) return;

            bool isCrouching = playerStatusManager.GetStatus(PlayerStatusType.IsCrouch);

            targetYOffset = isCrouching ? crouchYOffset : 0f;

            Vector3 currentPos = cameraTransform.localPosition;

            Vector3 targetPos = originalCameraLocalPos + new Vector3(0, targetYOffset, 0);

            cameraTransform.localPosition = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * smoothSpeed);
        }
    }
}