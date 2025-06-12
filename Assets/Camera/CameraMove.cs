using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

namespace kawanaka
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private PlayerStatusManager playerStatusManager;

        private void Update()
        {
            if (!playerStatusManager.GetStatus(PlayerStatusType.IsOperation))
            {
                // ëÄçÏíÜífíÜ
                return;
            }

            MoveCamera();
        }

        private void MoveCamera()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 moveDirection = (transform.right * horizontal + transform.forward * vertical).normalized;

            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
}