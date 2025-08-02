using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

public class YRotatorOnTrigger : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private float targetYRotation = 75f;
    [SerializeField] private float rotationSpeed = 2f;

    private bool shouldRotate = false;

    [Header("CloseSE")]
    [SerializeField] int CloseSENum;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && targetObject != null && !shouldRotate)
        {
            SEManager.Instance.PlaySE_Blocking(CloseSENum, SEManager.SECategory.Effect);
            shouldRotate = true;
        }
    }

    private void Update()
    {
        if (shouldRotate && targetObject != null)
        {
            Quaternion currentRotation = targetObject.rotation;
            Quaternion targetRotation = Quaternion.Euler(
                currentRotation.eulerAngles.x,
                targetYRotation,
                currentRotation.eulerAngles.z
            );

            targetObject.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);

            if (Quaternion.Angle(currentRotation, targetRotation) < 10.0f)
            {
                targetObject.rotation = targetRotation;
                shouldRotate = false;
                Destroy(gameObject);
            }
        }
    }
}