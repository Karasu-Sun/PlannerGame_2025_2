using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitalDialkey : MonoBehaviour
{
    [Header("�_�C�������b�Z�[�W�\���֘A")]
    public GameObject messagePanel;
    public Text messageText;

    [Header("�_�C���������\��")]
    public Text[] dialTexts = new Text[4]; // 0: L, 1: M, 2: R, 3: X

    private int[] dialNumbers = new int[4]; // �e���̐���

    void Start()
    {
        UpdateAllDials();
    }

    // �_�C�����̐����𑝂₷�iindex: 0�`3�j
    public void IncreaseDial(int index)
    {
        if (index < 0 || index >= dialNumbers.Length) return;

        dialNumbers[index] = (dialNumbers[index] + 1) % 10;
        UpdateDial(index);
    }

    // �_�C�����̐��������炷�iindex: 0�`3�j
    public void DecreaseDial(int index)
    {
        if (index < 0 || index >= dialNumbers.Length) return;

        dialNumbers[index] = (dialNumbers[index] + 9) % 10;
        UpdateDial(index);
    }

    // �_�C����1�̕\���X�V
    private void UpdateDial(int index)
    {
        if (dialTexts[index] != null)
        {
            dialTexts[index].text = dialNumbers[index].ToString();
        }
    }

    // �S�_�C�����X�V�i�������Ȃǁj
    private void UpdateAllDials()
    {
        for (int i = 0; i < dialNumbers.Length; i++)
        {
            UpdateDial(i);
        }
    }

    // Enter�{�^�����������Ƃ��̏���
    public void PushDialEnter()
    {
        if (dialNumbers[0] == 1 &&
            dialNumbers[1] == 2 &&
            dialNumbers[2] == 3 &&
            dialNumbers[3] == 4)
        {
            DisplayMessage("������");
        }
        else
        {
            DisplayMessage("�s������");
        }
    }

    // ���b�Z�[�W�\��
    void DisplayMessage(string message)
    {
        messagePanel.SetActive(true);
        messageText.text = message;
    }

    // ���b�Z�[�W��\��
    public void CloseMessage()
    {
        messagePanel.SetActive(false);
    }
}
