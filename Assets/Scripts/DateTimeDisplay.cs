using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace kawanaka
{
    public class DateTimeDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text dateTimeText;

        void Update()
        {
            DateTime now = DateTime.Now;
            string formatted = now.ToString("yyyy.MM.dd.HH.mm.ss");
            dateTimeText.text = formatted;
        }
    }
}