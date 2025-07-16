using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverTextEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text buttonText;
    private Color defaultColor;
    private FontStyle defaultStyle;

    private void Start()
    {
        buttonText = GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            defaultColor = buttonText.color;
            defaultStyle = buttonText.fontStyle;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.color = Color.red;
            buttonText.fontStyle = FontStyle.Bold;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.color = defaultColor;
            buttonText.fontStyle = defaultStyle;
        }
    }
}
