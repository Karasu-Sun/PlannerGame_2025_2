using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using cakeslice;
using Unity.VisualScripting;
using UnityEngine;

namespace kawanaka
{
    public class Tester_KK : MonoBehaviour
    {
        //[SerializeField] private CameraGlitch cameraGlitch;
        //[SerializeField] private SEManager sEManager;

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        if (sEManager.IsPlayingSE) return;

        //        SEManager.Instance.PlaySE_Blocking(0);
        //        cameraGlitch.TriggerGlitch(0.1f);
        //    }
        //}

        [SerializeField] private Outline targetComponent;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                targetComponent.eraseRenderer = !targetComponent.eraseRenderer;
            }
        }
    }
}