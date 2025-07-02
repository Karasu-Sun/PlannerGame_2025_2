using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace kawanaka
{
    public class TypewriterText : MonoBehaviour
    {
        [Header("�\���Ώ�")]
        [SerializeField] private TextMeshProUGUI targetText;

        [Header("�\�����x�i1����������j")]
        [SerializeField] private float typeSpeed = 0.05f;

        [Header("�\���ێ�����")]
        [SerializeField] private float displayDuration = 2f;

        [Header("�e�L�X�g�ꗗ")]
        [TextArea(2, 5)]
        [SerializeField] private List<string> messageList = new List<string>();

        private string currentMessage = "";
        private int charIndex = 0;
        private float typeTimer = 0f;
        private float displayTimer = 0f;
        private bool isTyping = false;

        // �\���J�n�i�C���f�b�N�X�w��j
        public void StartTypingByIndex(int index)
        {
            if (index < 0 || index >= messageList.Count)
            {
                Debug.LogWarning($"�w�肳�ꂽ�C���f�b�N�X {index} �͖����ł��B");
                return;
            }

            StartTyping(messageList[index]);
        }

        // �\���J�n�i���ڎw��j
        public void StartTyping(string message)
        {
            currentMessage = message;
            charIndex = 0;
            typeTimer = 0f;
            displayTimer = 0f;
            isTyping = true;

            targetText.text = "";
        }

        private void Update()
        {
            if (isTyping)
            {
                typeTimer += Time.deltaTime;
                if (typeTimer >= typeSpeed && charIndex < currentMessage.Length)
                {
                    targetText.text += currentMessage[charIndex];
                    charIndex++;
                    typeTimer = 0f;
                }

                // �S���\��������
                if (charIndex >= currentMessage.Length)
                {
                    displayTimer += Time.deltaTime;
                    if (displayTimer >= displayDuration)
                    {
                        ClearText();
                    }
                }
            }
        }

        public void ClearText()
        {
            targetText.text = "";
            currentMessage = "";
            isTyping = false;
            charIndex = 0;
            typeTimer = 0f;
            displayTimer = 0f;
        }
    }
}