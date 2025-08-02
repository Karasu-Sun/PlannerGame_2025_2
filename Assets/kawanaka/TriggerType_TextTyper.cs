using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

public class TriggerType_TextTyper : MonoBehaviour
{
    [SerializeField] private TypewriterText typewriterText;
    [SerializeField] private int TextNum;

    [SerializeField] private NonActiveFlagTrigger nonActiveFlagTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // テキスト表示
            typewriterText.StartTypingByIndex(TextNum);
            Destroy(gameObject);
        }
    }

    private void Update()
    {

        if (nonActiveFlagTrigger.nonActiveFlag)
        {
            Destroy(gameObject);
        }
    }
}
