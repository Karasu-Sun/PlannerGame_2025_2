using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitalDialkey : MonoBehaviour
{
    [Header("ダイヤルメッセージ表示関連")]
    public GameObject messagePanel;
    public Text messageText;

    [Header("ダイヤル数字表示")]
    public Text[] dialTexts = new Text[4]; // 0: L, 1: M, 2: R, 3: X

    private int[] dialNumbers = new int[4]; // 各桁の数字

    void Start()
    {
        UpdateAllDials();
    }

    // ダイヤルの数字を増やす（index: 0～3）
    public void IncreaseDial(int index)
    {
        if (index < 0 || index >= dialNumbers.Length) return;

        dialNumbers[index] = (dialNumbers[index] + 1) % 10;
        UpdateDial(index);
    }

    // ダイヤルの数字を減らす（index: 0～3）
    public void DecreaseDial(int index)
    {
        if (index < 0 || index >= dialNumbers.Length) return;

        dialNumbers[index] = (dialNumbers[index] + 9) % 10;
        UpdateDial(index);
    }

    // ダイヤル1つの表示更新
    private void UpdateDial(int index)
    {
        if (dialTexts[index] != null)
        {
            dialTexts[index].text = dialNumbers[index].ToString();
        }
    }

    // 全ダイヤル更新（初期化など）
    private void UpdateAllDials()
    {
        for (int i = 0; i < dialNumbers.Length; i++)
        {
            UpdateDial(i);
        }
    }

    // Enterボタンを押したときの処理
    public void PushDialEnter()
    {
        if (dialNumbers[0] == 1 &&
            dialNumbers[1] == 2 &&
            dialNumbers[2] == 3 &&
            dialNumbers[3] == 4)
        {
            DisplayMessage("正解▼");
        }
        else
        {
            DisplayMessage("不正解▼");
        }
    }

    // メッセージ表示
    void DisplayMessage(string message)
    {
        messagePanel.SetActive(true);
        messageText.text = message;
    }

    // メッセージ非表示
    public void CloseMessage()
    {
        messagePanel.SetActive(false);
    }
}
