using System.Collections;
using System.Collections.Generic;
using kawanaka;
using Unity.VisualScripting;
using UnityEngine;
using static kawanaka.SEManager;

public class Player_move_SE_Manager : MonoBehaviour
{
    [SerializeField] private PlayerStatusManager playerStatusManager;

    [Header("足音SEインデックス")]
    [SerializeField] private int walkSEIndex = 0;
    [SerializeField] private int runSEIndex = 1;
    [SerializeField] private int indoorWalkSEIndex = 2;

    [Header("室内判定")]
    [SerializeField] private bool isIndoor = false;

    [Header("再生カテゴリ")]
    [SerializeField] private SEManager.SECategory footstepCategory = SEManager.SECategory.Footsteps;

    private int currentSEIndex = -1;
    private Coroutine currentSECoroutine = null;

    private void Update()
    {
        bool isWalking = playerStatusManager.GetStatus(PlayerStatusType.IsWalk);
        bool isSprinting = playerStatusManager.GetStatus(PlayerStatusType.IsSprint);

        int nextSEIndex = -1;

        if (isWalking)
        {
            if (isSprinting)
            {
                nextSEIndex = runSEIndex;
            }
            else if (isIndoor)
            {
                nextSEIndex = indoorWalkSEIndex;
            }
            else
            {
                nextSEIndex = walkSEIndex;
            }
        }

        // 足音の切り替え
        if (nextSEIndex != -1 && nextSEIndex != currentSEIndex)
        {
            if (currentSECoroutine != null)
                StopCoroutine(currentSECoroutine);

            currentSECoroutine = StartCoroutine(FadeOutAndPlaySE(nextSEIndex, footstepCategory, 0.3f));
            currentSEIndex = nextSEIndex;
        }

        // 移動していないとき、足音停止
        if (!isWalking && currentSEIndex != -1)
        {
            SEManager.Instance.StopSE(footstepCategory, 0.3f);
            currentSEIndex = -1;
        }
    }

    // SEManagerの新機能
    private IEnumerator FadeOutAndPlaySE(int seIndex, SEManager.SECategory category, float fadeTime)
    {
        SEManager.Instance.FadeOutAndPlaySE_Looping(seIndex, category, fadeTime);
        yield return new WaitForSeconds(fadeTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RoomAria"))
        {
            isIndoor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RoomAria"))
        {
            isIndoor = false;
        }
    }
}