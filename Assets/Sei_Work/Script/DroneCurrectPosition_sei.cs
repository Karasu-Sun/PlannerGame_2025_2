using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sei_kawanaka_Fix;
using System.Drawing;
using UnityEngine.UIElements;

// �h���[���N�����Ɉʒu���C�����邾���̃X�N���v�g

namespace sei
{
    public class DroneCorrectPosition_sei : MonoBehaviour
    {
        [SerializeField] private DroneActivator droneActivator;

        [Header("�L�����Ώ�")]
        [SerializeField] private Camera droneCamera;

        [Header("Y���ʒu")]
        [SerializeField] float cameraY = 30.0f;

        [Header("�v���C���[�Ƃ���ʒu")]
        [SerializeField] private GameObject player;

        //DroneMoveLimiter_sei����̕ϐ��̎󂯓n��
        private DroneMoveLimiter_sei droneMoveLimiter_Sei;
        float maxDistans;
        private Vector3 dronePosition;
        //�h���[�����삪���߂Ă��ǂ���
        private bool farstDroneOperate = false;
        //�h���[���N�����̃|�W�V��������x�����ς��邽�߂̃t���O
        public bool dronePositionSet = false;

        private void Start()
        {
            droneMoveLimiter_Sei = GetComponent<DroneMoveLimiter_sei>();
            maxDistans = droneMoveLimiter_Sei.maxDistance;
        }
        private void Update()
        {
            dronePosition = droneCamera.transform.position;
            float distance = Vector3.Distance(dronePosition, player.transform.position);
            
            if(!dronePositionSet && droneActivator.isOperation)
            {
                if(!farstDroneOperate)
                {
                    droneCamera.transform.position = new Vector3(player.transform.position.x, cameraY, player.transform.position.z);
                    farstDroneOperate = true;
                }
                else if(distance <= maxDistans)
                {
                    droneCamera.transform.position = dronePosition;
                }
                else if(distance > maxDistans)
                {
                    droneCamera.transform.position = new Vector3(player.transform.position.x, cameraY, player.transform.position.z);
                }
                dronePositionSet = true;
            }
            if(!droneActivator.isOperation)
            {
                dronePositionSet = false;
            }
        }
    }
}