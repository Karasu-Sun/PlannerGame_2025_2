using System;
using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;
using static kawanaka.SEManager;

public class SE_Relay : MonoBehaviour
{
    [Header("çƒê∂Ç∑ÇÈSEÇÃê›íË")]
    [SerializeField] private int seIndex;
    [SerializeField] private SECategory category = SECategory.System;

    public void PlaySE_Relay()
    {
        SEManager.Instance.PlaySE(seIndex, category);
    }
}
