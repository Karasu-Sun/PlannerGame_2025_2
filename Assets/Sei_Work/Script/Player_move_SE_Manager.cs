using System.Collections;
using System.Collections.Generic;
using kawanaka;
using Unity.VisualScripting;
using UnityEngine;
using static kawanaka.SEManager;

public class Player_move_SE_Manager : MonoBehaviour
{
    [SerializeField]
    private PlayerStatusManager playerStatusManager;
    

    [SerializeField] private bool concrete = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStatusManager.GetStatus(PlayerStatusType.IsWalk))
        {
            if(concrete) StartCoroutine(FadeOutAndPlaySE(12, SEManager.SECategory.Footsteps, 0.5f));
            else StartCoroutine(FadeOutAndPlaySE(13, SEManager.SECategory.Footsteps, 0.5f));
            Debug.Log("walk");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoomAria"))
        {
            concrete = true;
            // SEを再生する処理
            Debug.Log("コンクリート");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RoomAria"))
        {
            concrete = false;
            // SEを停止する処理
            Debug.Log("コンクリートから出た");
        }
    }
    private IEnumerator FadeOutAndPlaySE(int seIndex, SECategory category, float fadeTime)
    {
        SEManager.Instance.StopSE(category, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        SEManager.Instance.PlaySE_Looping(seIndex, category);
    }
}
