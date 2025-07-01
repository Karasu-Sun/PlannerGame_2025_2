using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

namespace kawanaka
{
    public class SprintVerticalBob : MonoBehaviour
    {
        [SerializeField] private PlayerStatusManager playerStatusManager;
        [SerializeField] private PlayerStatusType sprintStatus = PlayerStatusType.IsSprint;
        [SerializeField] private PlayerStatusType walkStatus = PlayerStatusType.IsWalk;
        [SerializeField] private PlayerStatusType crouchStatus = PlayerStatusType.IsCrouch;

        [Header("ÉXÉvÉäÉìÉgóhÇÍê›íË")]
        [SerializeField] private float sprintBobAmplitude = 0.1f;
        [SerializeField] private float sprintBobFrequency = 10f;

        [Header("ï‡çsóhÇÍê›íË")]
        [SerializeField] private float walkBobAmplitude = 0.06f;
        [SerializeField] private float walkBobFrequency = 6f;

        [Header("ÇµÇ·Ç™Ç›óhÇÍê›íË")]
        [SerializeField] private float crouchBobAmplitude = 0.03f;
        [SerializeField] private float crouchBobFrequency = 4f;

        private float bobTimer = 0f;
        private Vector3 originalPosition;

        private void Start()
        {
            originalPosition = transform.localPosition;
        }

        private void Update()
        {
            if (playerStatusManager == null) return;

            bool isSprinting = playerStatusManager.GetStatus(sprintStatus);
            bool isWalking = playerStatusManager.GetStatus(walkStatus);
            bool isCrouching = playerStatusManager.GetStatus(crouchStatus);

            Vector3 currentPos = transform.localPosition;

            if (isSprinting)
            {
                bobTimer += Time.deltaTime * sprintBobFrequency;
                float bobOffset = Mathf.Sin(bobTimer) * sprintBobAmplitude;
                transform.localPosition = new Vector3(currentPos.x, originalPosition.y + bobOffset, currentPos.z);
            }
            else if (isWalking)
            {
                bobTimer += Time.deltaTime * walkBobFrequency;
                float bobOffset = Mathf.Sin(bobTimer) * walkBobAmplitude;
                transform.localPosition = new Vector3(currentPos.x, originalPosition.y + bobOffset, currentPos.z);
            }
            else if (isCrouching)
            {
                bobTimer += Time.deltaTime * crouchBobFrequency;
                float bobOffset = Mathf.Sin(bobTimer) * crouchBobAmplitude;
                transform.localPosition = new Vector3(currentPos.x, originalPosition.y + bobOffset, currentPos.z);
            }
            else
            {
                // îÒà⁄ìÆéûÇÕå≥ÇÃà íuÇ…ñﬂÇ∑
                bobTimer = 0f;
                Vector3 targetPos = new Vector3(currentPos.x, originalPosition.y, currentPos.z);
                transform.localPosition = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * 10f);
            }
        }
    }
}