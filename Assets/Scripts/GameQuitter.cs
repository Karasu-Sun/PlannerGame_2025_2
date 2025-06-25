using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace kawanaka
{
    public class GameQuitter : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                QuitGame();
            }

            if (Input.GetKey(KeyCode.JoystickButton4) && Input.GetKey(KeyCode.JoystickButton5))
            {
                QuitGame();
            }
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}