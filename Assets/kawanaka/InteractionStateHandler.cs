using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace kawanaka
{
    public class InteractionStateHandler : MonoBehaviour
    {
        [Header("参照")]
        [SerializeField] private PlayerStatusManager playerStatusManager;

        [Header("変更対象スクリプト")]
        [SerializeField] private MouseRotationH targetScript;

        [Tooltip("変更する rotationSpeed の値")]
        public float newRotationSpeed = 0f;

        [Tooltip("変更する RotationY の値")]
        public float newRotationY = 90f;

        private bool hasModified = false;

        private FieldInfo rotationSpeedField;
        private FieldInfo rotationYField;

        private void Start()
        {
            if (targetScript != null)
            {
                var type = typeof(MouseRotationH);

                rotationSpeedField = type.GetField("rotationSpeed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                rotationYField = type.GetField("rotationY", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            }
        }

        private float originalRotationSpeed;
        private bool hasCachedOriginal = false;

        private void Update()
        {
            if (playerStatusManager == null || targetScript == null)
                return;

            bool isInteracting = playerStatusManager.GetStatus(PlayerStatusType.IsInteracting);

            if (isInteracting)
            {
                if (!hasModified)
                {
                    if (!hasCachedOriginal)
                    {
                        if (rotationSpeedField != null)
                            originalRotationSpeed = (float)rotationSpeedField.GetValue(targetScript);

                        hasCachedOriginal = true;
                    }

                    if (rotationSpeedField != null)
                        rotationSpeedField.SetValue(targetScript, newRotationSpeed);

                    if (rotationYField != null)
                        rotationYField.SetValue(targetScript, newRotationY);

                    hasModified = true;
                }
            }
            else
            {
                if (hasModified && hasCachedOriginal)
                {
                    if (rotationSpeedField != null)
                        rotationSpeedField.SetValue(targetScript, originalRotationSpeed);
                }

                hasModified = false;
                hasCachedOriginal = false;
            }
        }
    }
}