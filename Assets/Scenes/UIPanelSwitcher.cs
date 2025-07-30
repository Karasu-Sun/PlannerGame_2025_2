using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class UIPanelSwitcher : MonoBehaviour
    {
        [Header("êÿÇËë÷Ç¶ëŒè€ÇÃÉpÉlÉã")]
        [SerializeField] private GameObject panel_GamePlay;
        [SerializeField] private GameObject panel_Graphics;
        [SerializeField] private GameObject panel_Audio;

        public void OpenPanelGamePlay()
        {
            panel_GamePlay.SetActive(true);
            panel_Graphics.SetActive(false);
            panel_Audio.SetActive(false);
        }

        public void OpenPanelGraphics()
        {
            panel_Graphics.SetActive(true);
            panel_GamePlay.SetActive(false);
            panel_Audio.SetActive(false);
        }
        public void OpenPanelAudio()
        {
            panel_Audio.SetActive(true);
            panel_GamePlay.SetActive(false);
            panel_Graphics.SetActive(false);
        }
    }
}