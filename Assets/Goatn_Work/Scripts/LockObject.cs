using System.Collections;
using System.Collections.Generic;
using kawanaka;
using UnityEngine;

public class LockObject : MonoBehaviour
{
    [SerializeField] private TypewriterText typewriterText;
    [SerializeField] private int LockTextNum;
    [SerializeField] private int UnlockTextNum;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            // 鍵を所持していれば開錠
            if (collision.gameObject.GetComponent<GetUnlockKey>().keys > 0)
            {
                collision.gameObject.GetComponent<GetUnlockKey>().keys--;
                collision.gameObject.GetComponent<GetUnlockKey>().UpdateKeyUI();

                // テキスト表示
                typewriterText.StartTypingByIndex(UnlockTextNum);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("鍵が無いので開錠できない");

                // テキスト表示
                typewriterText.StartTypingByIndex(LockTextNum);
            }
        }
    }
}
