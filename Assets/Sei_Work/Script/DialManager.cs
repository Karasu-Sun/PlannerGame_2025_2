using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialManager : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> digit;
    private int digitCurrent = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (digitCurrent < 3)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                digitCurrent++;
                Debug.Log(digitCurrent);
            }
        }
        if (digitCurrent > 0)
        {
            if(Input.GetKeyDown(KeyCode.D))
            {
                digitCurrent--;
                Debug.Log(digitCurrent);
            }
        }
    }
}
