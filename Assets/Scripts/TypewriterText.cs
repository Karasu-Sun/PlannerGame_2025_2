using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace kawanaka
{
    public class TypewriterText : MonoBehaviour
    {
        [Header("表示対象")]
        [SerializeField] private TextMeshProUGUI targetText;

        [Header("表示速度（1文字あたり）")]
        [SerializeField] private float typeSpeed = 0.05f;

        [Header("表示維持時間")]
        [SerializeField] private float displayDuration = 2f;

        [Header("テキスト一覧")]
        [TextArea(2, 5)]
        [SerializeField] private List<string> messageList = new List<string>();

        private string currentMessage = "";
        private int charIndex = 0;
        private float typeTimer = 0f;
        private float displayTimer = 0f;
        private bool isTyping = false;

        // 表示開始（インデックス指定）
        public void StartTypingByIndex(int index)
        {
            if (index < 0 || index >= messageList.Count)
            {
                Debug.LogWarning($"指定されたインデックス {index} は無効です。");
                return;
            }

            StartTyping(messageList[index]);
        }

        // 表示開始（直接指定）
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

                // 全文表示完了時
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