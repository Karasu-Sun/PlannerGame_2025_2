using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace kawanaka
{
    public class OptionActivator : MonoBehaviour
    {
        [SerializeField] private VolumeControl volumeControl;

        [SerializeField] private PlayerStatusManager playerStatusManager;

        [SerializeField] private bool isPausing = false;

        [SerializeField] private bool previousPausingState = false;

        [SerializeField] private SEManager sEManager;

        //[SerializeField] private MouseLock mouseLock;

        private void Update()
        {
            isPausing = playerStatusManager.GetStatus(PlayerStatusType.IsOption);

            if (sEManager.IsPlayingSE) return;
            if (playerStatusManager.GetStatus(PlayerStatusType.IsOperation)) return;

            if (isPausing != previousPausingState)
            {
                Time.timeScale = isPausing ? 0 : 1;
                volumeControl.ToggleVolumePanel();

                // ÉJÅ[É\Éãêßå‰
                //if (isPausing)
                //{
                //    mouseLock.UnlockCursor();
                //}
                //else
                //{
                //    mouseLock.LockCursor();
                //}

                previousPausingState = isPausing;
            }
        }
    }
}